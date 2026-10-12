using FleetManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Web.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private static readonly string[] AllRoles = { "admin", "manager", "driver", "viewer" };
        private static readonly string[] ManagerRoles = { "driver", "viewer" };

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // =========================================================
        // READ — all users
        // =========================================================
        public async Task<ServiceResult<List<UserResponse>>> GetAllAsync()
        {
            var users = await _userManager.Users.OrderBy(u => u.Email).ToListAsync();
            var list = new List<UserResponse>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                list.Add(Map(u, roles));
            }
            return ServiceResult<List<UserResponse>>.Ok(list);
        }

        // =========================================================
        // READ — only drivers + viewers (for manager)
        // =========================================================
        public async Task<ServiceResult<List<UserResponse>>> GetForManagerAsync(string actorEmail)
        {
            var users = await _userManager.Users.OrderBy(u => u.Email).ToListAsync();
            var list = new List<UserResponse>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);

                var isSelf = u.Email == actorEmail;
                var isManageable = roles.Any(r => ManagerRoles.Contains(r));

                if (isSelf || isManageable)
                    list.Add(Map(u, roles));
            }

            return ServiceResult<List<UserResponse>>.Ok(list);
        }

        // =========================================================
        // READ — single
        // =========================================================
        public async Task<ServiceResult<UserResponse>> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return ServiceResult<UserResponse>.Fail("User not found.");

            var roles = await _userManager.GetRolesAsync(user);
            return ServiceResult<UserResponse>.Ok(Map(user, roles));
        }

        // =========================================================
        // CREATE
        // =========================================================
        public async Task<ServiceResult<UserResponse>> CreateAsync(CreateUserRequest req)
        {
            var role = req.Role.ToLower().Trim();
            if (!AllRoles.Contains(role))
                return ServiceResult<UserResponse>.Fail("Invalid role.");

            if (await _userManager.FindByEmailAsync(req.Email) != null)
                return ServiceResult<UserResponse>.Fail("Email already registered.");

            var user = new ApplicationUser
            {
                UserName = req.Email,
                Email = req.Email,
                EmailConfirmed = true,
                Name = req.Name,
                PhoneNumber = req.Phone,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var create = await _userManager.CreateAsync(user, req.Password);
            if (!create.Succeeded)
                return ServiceResult<UserResponse>.Fail(create.Errors.Select(e => e.Description).ToArray());

            await _userManager.AddToRoleAsync(user, role);

            var roles = await _userManager.GetRolesAsync(user);
            return ServiceResult<UserResponse>.Ok(Map(user, roles));
        }

        // =========================================================
        // UPDATE
        // =========================================================
        public async Task<ServiceResult<UserResponse>> UpdateAsync(UpdateUserRequest req)
        {
            var user = await _userManager.FindByIdAsync(req.Id);
            if (user == null) return ServiceResult<UserResponse>.Fail("User not found.");

            var newRole = req.Role.ToLower().Trim();
            if (!AllRoles.Contains(newRole))
                return ServiceResult<UserResponse>.Fail("Invalid role.");

            if (user.Email != req.Email)
            {
                var existing = await _userManager.FindByEmailAsync(req.Email);
                if (existing != null && existing.Id != user.Id)
                    return ServiceResult<UserResponse>.Fail("Email already in use.");

                user.Email = req.Email;
                user.UserName = req.Email;
                user.NormalizedEmail = req.Email.ToUpper();
                user.NormalizedUserName = req.Email.ToUpper();
            }

            user.Name = req.Name;
            user.PhoneNumber = req.Phone;
            user.IsActive = req.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded)
                return ServiceResult<UserResponse>.Fail(update.Errors.Select(e => e.Description).ToArray());

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(newRole))
            {
                if (currentRoles.Any())
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, newRole);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return ServiceResult<UserResponse>.Ok(Map(user, roles));
        }

        // =========================================================
        // TOGGLE ACTIVE
        // =========================================================
        public async Task<ServiceResult> ToggleActiveAsync(string id, string actorEmail)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return ServiceResult.Fail("User not found.");

            if (user.Email == actorEmail)
                return ServiceResult.Fail("You cannot deactivate your own account.");

            user.IsActive = !user.IsActive;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return ServiceResult.Ok();
        }

        // =========================================================
        // RESET PASSWORD
        // =========================================================
        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest req)
        {
            var user = await _userManager.FindByIdAsync(req.UserId);
            if (user == null) return ServiceResult.Fail("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, req.NewPassword);
            if (!result.Succeeded)
                return ServiceResult.Fail(result.Errors.Select(e => e.Description).ToArray());

            return ServiceResult.Ok();
        }

        // =========================================================
        // DELETE
        // =========================================================
        public async Task<ServiceResult> DeleteAsync(string id, string actorEmail)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return ServiceResult.Fail("User not found.");

            if (user.Email == actorEmail)
                return ServiceResult.Fail("You cannot delete your own account.");

            if (await _userManager.IsInRoleAsync(user, "admin"))
            {
                var admins = await _userManager.GetUsersInRoleAsync("admin");
                if (admins.Count <= 1)
                    return ServiceResult.Fail("Cannot delete the last admin account.");
            }

            await _userManager.DeleteAsync(user);
            return ServiceResult.Ok();
        }

        // =========================================================
        // MAPPER
        // =========================================================
        private static UserResponse Map(ApplicationUser u, IList<string> roles) => new()
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email ?? "",
            Phone = u.PhoneNumber,
            IsActive = u.IsActive,
            Roles = roles.ToList(),
            LastLoginAt = u.LastLoginAt,
            CreatedAt = u.CreatedAt
        };

        // =========================================================
// GET OWN PROFILE
// =========================================================
public async Task<ServiceResult<UserResponse>> GetProfileAsync(string actorEmail)
{
    var user = await _userManager.FindByEmailAsync(actorEmail);
    if (user == null) return ServiceResult<UserResponse>.Fail("Profile not found.");

    var roles = await _userManager.GetRolesAsync(user);
    return ServiceResult<UserResponse>.Ok(Map(user, roles));
}

// =========================================================
// UPDATE OWN PROFILE (name + phone only)
// =========================================================
    public async Task<ServiceResult<UserResponse>> UpdateProfileAsync(UpdateProfileRequest req, string actorEmail)
    {
        var user = await _userManager.FindByEmailAsync(actorEmail);
        if (user == null) return ServiceResult<UserResponse>.Fail("Profile not found.");

        // Safety: only allow updating your OWN record
        if (user.Id != req.Id)
            return ServiceResult<UserResponse>.Fail("You can only update your own profile.");

        user.Name = req.Name;
        user.PhoneNumber = req.Phone;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return ServiceResult<UserResponse>.Fail(result.Errors.Select(e => e.Description).ToArray());

        var roles = await _userManager.GetRolesAsync(user);
        return ServiceResult<UserResponse>.Ok(Map(user, roles));
    }


    }
}
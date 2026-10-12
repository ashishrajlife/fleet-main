using FleetManagement.Web.Models;
using FleetManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Web.Controllers
{
    [Authorize(Roles = "manager")]
    public class ManagerController : Controller
    {
        private readonly IUserService _users;
        private static readonly string[] ManageableRoles = { "driver", "viewer" };

        public ManagerController(IUserService users) => _users = users;

        public IActionResult Dashboard() => View();

        // =========================================================
        // LIST — drivers + viewers + self
        // =========================================================
        public async Task<IActionResult> UserList()
        {
            var result = await _users.GetForManagerAsync(User.Identity?.Name ?? "");
            if (!result.Succeeded) return BadRequest(result.Errors);
            return View(result.Data);
        }

        // =========================================================
        // CREATE — driver/viewer only
        // =========================================================
        [HttpGet]
        public IActionResult CreateUser() => View(new CreateUserRequest { Role = "driver" });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserRequest req)
        {
            if (!ModelState.IsValid) return View(req);

            if (!ManageableRoles.Contains(req.Role.ToLower()))
            {
                ModelState.AddModelError("Role", "You can only create drivers or viewers.");
                return View(req);
            }

            var result = await _users.CreateAsync(req);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors) ModelState.AddModelError("", err);
                return View(req);
            }

            TempData["Success"] = $"User '{req.Name}' created.";
            return RedirectToAction("UserList");
        }

        // =========================================================
        // EDIT — only driver/viewer (self edit is separate)
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var result = await _users.GetByIdAsync(id);
            if (!result.Succeeded) return NotFound();

            var u = result.Data!;
            if (!u.Roles.Any(r => ManageableRoles.Contains(r))) return Forbid();

            return View(new UpdateUserRequest
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Roles.FirstOrDefault() ?? "viewer",
                IsActive = u.IsActive
            });
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditUser(UpdateUserRequest req)
{
    if (!ModelState.IsValid) return View(req);

    if (!ManageableRoles.Contains(req.Role.ToLower()))
    {
        ModelState.AddModelError("Role", "You can only assign driver or viewer.");
        return View(req);
    }

    var target = await _users.GetByIdAsync(req.Id);
    if (!target.Succeeded) return NotFound();
    if (!target.Data!.Roles.Any(r => ManageableRoles.Contains(r))) return Forbid();

    var result = await _users.UpdateAsync(req, User.Identity?.Name ?? "");
    if (!result.Succeeded)
    {
        foreach (var err in result.Errors) ModelState.AddModelError("", err);
        return View(req);
    }

    TempData["Success"] = "User updated.";
    return RedirectToAction("UserList");
}

        // =========================================================
        // TOGGLE ACTIVE — driver/viewer only
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(string id)
        {
            var check = await _users.GetByIdAsync(id);
            if (!check.Succeeded) return NotFound();
            if (!check.Data!.Roles.Any(r => ManageableRoles.Contains(r))) return Forbid();

            var actorEmail = User.Identity?.Name ?? "";
            var result = await _users.ToggleActiveAsync(id, actorEmail);
            if (!result.Succeeded) TempData["Error"] = string.Join(" ", result.Errors);
            else TempData["Success"] = "Status changed.";

            return RedirectToAction("UserList");
        }

        // =========================================================
        // RESET PASSWORD — driver/viewer only
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var check = await _users.GetByIdAsync(id);
            if (!check.Succeeded) return NotFound();
            if (!check.Data!.Roles.Any(r => ManageableRoles.Contains(r))) return Forbid();

            return View(new ResetPasswordRequest { UserId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest req)
        {
            if (!ModelState.IsValid) return View(req);

            var check = await _users.GetByIdAsync(req.UserId);
            if (!check.Succeeded) return NotFound();
            if (!check.Data!.Roles.Any(r => ManageableRoles.Contains(r))) return Forbid();

            var result = await _users.ResetPasswordAsync(req);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors) ModelState.AddModelError("", err);
                return View(req);
            }

            TempData["Success"] = "Password reset.";
            return RedirectToAction("UserList");
        }

  }
}
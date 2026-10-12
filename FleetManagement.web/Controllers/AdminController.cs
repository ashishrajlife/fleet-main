using FleetManagement.Web.Models;
using FleetManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _users;

        public AdminController(IUserService users) => _users = users;

        public IActionResult Dashboard() => View();

        // ---------------- LIST ----------------
        public async Task<IActionResult> UserList()
        {
            var result = await _users.GetAllAsync();
            if (!result.Succeeded) return BadRequest(result.Errors);
            return View(result.Data);
        }

        // ---------------- CREATE ----------------
        [HttpGet]
        public IActionResult CreateUser() => View(new CreateUserRequest());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserRequest req)
        {
            if (!ModelState.IsValid) return View(req);

            var result = await _users.CreateAsync(req);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors) ModelState.AddModelError("", err);
                return View(req);
            }

            TempData["Success"] = $"User '{req.Name}' created with role '{req.Role}'.";
            return RedirectToAction("UserList");
        }

        // ---------------- EDIT ----------------
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var result = await _users.GetByIdAsync(id);
            if (!result.Succeeded) return NotFound();

            var u = result.Data!;
            var vm = new UpdateUserRequest
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Roles.FirstOrDefault() ?? "viewer",
                IsActive = u.IsActive
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UpdateUserRequest req)
        {
            if (!ModelState.IsValid) return View(req);

            var result = await _users.UpdateAsync(req, User.Identity?.Name ?? "");
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors) ModelState.AddModelError("", err);
                return View(req);
            }

            TempData["Success"] = "User updated.";
            return RedirectToAction("UserList");
        }

        // ---------------- TOGGLE ACTIVE ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(string id)
        {
            var actorEmail = User.Identity?.Name ?? "";
            var result = await _users.ToggleActiveAsync(id, actorEmail);
            if (!result.Succeeded)
                TempData["Error"] = string.Join(" ", result.Errors);
            else
                TempData["Success"] = "Status changed.";

            return RedirectToAction("UserList");
        }

        // ---------------- RESET PASSWORD ----------------
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var result = await _users.GetByIdAsync(id);
            if (!result.Succeeded) return NotFound();

            return View(new ResetPasswordRequest { UserId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest req)
        {
            if (!ModelState.IsValid) return View(req);

            var result = await _users.ResetPasswordAsync(req);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors) ModelState.AddModelError("", err);
                return View(req);
            }

            TempData["Success"] = "Password reset.";
            return RedirectToAction("UserList");
        }

        // ---------------- DELETE ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var actorEmail = User.Identity?.Name ?? "";
            var result = await _users.DeleteAsync(id, actorEmail);
            if (!result.Succeeded)
                TempData["Error"] = string.Join(" ", result.Errors);
            else
                TempData["Success"] = "User deleted.";

            return RedirectToAction("UserList");
        }

    }
    
}
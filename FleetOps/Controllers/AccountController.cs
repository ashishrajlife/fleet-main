using FleetOps.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FleetOps.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
            return RedirectToLocal(returnUrl);

        if (result.IsLockedOut)
            ModelState.AddModelError("", "Account locked. Try again later.");
        else
            ModelState.AddModelError("", "Invalid email or password.");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction("Index", "Home");
    }

    // TEMPORARY — run ONCE, then delete this action.
    [HttpGet("Account/SetupAdmin")]
    public async Task<IActionResult> SetupAdmin()
    {
        var email = "admin@fleetops.com";
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var create = await _userManager.CreateAsync(user, "Admin@123");
            if (!create.Succeeded)
                return Content("Failed: " + string.Join("; ", create.Errors.Select(e => e.Description)));

            var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roleManager.RoleExistsAsync("Administrator"))
                await roleManager.CreateAsync(new IdentityRole("Administrator"));

            await _userManager.AddToRoleAsync(user, "Administrator");
            return Content("Admin created.");
        }
        return Content("Admin already exists.");
    }
}
using FleetManagement.Web.Models;
using FleetManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace FleetManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _auth;
        private readonly IConfiguration _config;

        public AccountController(IAuthService auth, IConfiguration config)
        {
            _auth = auth;
            _config = config;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest req, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(req);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var ua = Request.Headers["User-Agent"].ToString();

            var result = await _auth.LoginAsync(req, ip, ua);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Errors.FirstOrDefault() ?? "Login failed.");
                return View(req);
            }

            var data = result.Data!;
            Response.Cookies.Append("access_token", data.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = data.AccessTokenExpiresAt
            });

            Response.Cookies.Append("refresh_token", data.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenDays"] ?? "7"))
            });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var rawRefresh = Request.Cookies["refresh_token"];
            var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var expClaim = User.FindFirst("exp")?.Value;
            var userId = User.FindFirst("uid")?.Value;

            DateTime? expiry = null;
            if (long.TryParse(expClaim, out var unix))
                expiry = DateTimeOffset.FromUnixTimeSeconds(unix).UtcDateTime;

            await _auth.LogoutAsync(rawRefresh, jti, userId, expiry);

            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
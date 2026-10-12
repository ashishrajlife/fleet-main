using FleetManagement.Web.Models;
using FleetManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace FleetManagement.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var ua = Request.Headers["User-Agent"].ToString();

            var result = await _auth.LoginAsync(req, ip, ua);
            if (!result.Succeeded) return Unauthorized(new { message = result.Errors.FirstOrDefault() });

            return Ok(result.Data);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var ua = Request.Headers["User-Agent"].ToString();

            var result = await _auth.RefreshAsync(req.RefreshToken, ip, ua);
            if (!result.Succeeded) return Unauthorized(new { message = result.Errors.FirstOrDefault() });

            return Ok(result.Data);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest req)
        {
            var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var expClaim = User.FindFirst("exp")?.Value;
            var userId = User.FindFirst("uid")?.Value;

            DateTime? expiry = null;
            if (long.TryParse(expClaim, out var unix))
                expiry = DateTimeOffset.FromUnixTimeSeconds(unix).UtcDateTime;

            await _auth.LogoutAsync(req.RefreshToken, jti, userId, expiry);
            return Ok(new { message = "Logged out." });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _auth.MeAsync(userId);
            if (!result.Succeeded) return Unauthorized();

            return Ok(result.Data);
        }

        [HttpGet("admin/panel")]
        [Authorize(Roles = "admin")]
        public IActionResult AdminPanel() => Ok(new { message = "Welcome, admin.", area = "admin" });

        [HttpGet("manager/panel")]
        [Authorize(Roles = "manager")]
        public IActionResult ManagerPanel() => Ok(new { message = "Welcome, manager.", area = "manager" });

        [HttpGet("driver/panel")]
        [Authorize(Roles = "driver")]
        public IActionResult DriverPanel() => Ok(new { message = "Welcome, driver.", area = "driver" });
    }
}
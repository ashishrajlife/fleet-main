using FleetManagement.Web.Data;
using FleetManagement.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace FleetManagement.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _db;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _db = db;
        }

        public async Task<ServiceResult<LoginResponse>> LoginAsync(LoginRequest req, string? ip, string? userAgent)
        {
            var user = await _userManager.FindByEmailAsync(req.Email);
            if (user == null || !user.IsActive)
                return ServiceResult<LoginResponse>.Fail("Invalid credentials.");

            var check = await _signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);
            if (!check.Succeeded)
                return ServiceResult<LoginResponse>.Fail("Invalid credentials.");

            user.LastLoginAt = DateTime.UtcNow;
            user.LastLoginIp = ip;
            await _userManager.UpdateAsync(user);

            return ServiceResult<LoginResponse>.Ok(await BuildLoginResponseAsync(user, ip, userAgent));
        }

        public async Task<ServiceResult<LoginResponse>> RefreshAsync(string rawRefreshToken, string? ip, string? userAgent)
        {
            var result = await _tokenService.ValidateRefreshTokenAsync(rawRefreshToken);
            if (result == null)
                return ServiceResult<LoginResponse>.Fail("Invalid refresh token.");

            var (user, oldToken) = result.Value;
            await _tokenService.RevokeRefreshTokenAsync(oldToken);

            return ServiceResult<LoginResponse>.Ok(await BuildLoginResponseAsync(user, ip, userAgent));
        }

        public async Task<ServiceResult> LogoutAsync(string? rawRefreshToken, string? jti, string? userId, DateTime? accessTokenExpiry)
        {
            if (!string.IsNullOrEmpty(rawRefreshToken))
            {
                var result = await _tokenService.ValidateRefreshTokenAsync(rawRefreshToken);
                if (result != null)
                    await _tokenService.RevokeRefreshTokenAsync(result.Value.token);
            }

            if (!string.IsNullOrEmpty(jti))
            {
                var expiresAt = accessTokenExpiry ?? DateTime.UtcNow.AddMinutes(15);
                await _tokenService.BlacklistJtiAsync(jti, userId, expiresAt, "logout");
            }

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult<LoginResponse>> MeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return ServiceResult<LoginResponse>.Fail("User not found.");

            var roles = await _userManager.GetRolesAsync(user);
            return ServiceResult<LoginResponse>.Ok(new LoginResponse
            {
                UserId = user.Id,
                Email = user.Email ?? "",
                Name = user.Name,
                Roles = roles.ToList()
            });
        }

        private async Task<LoginResponse> BuildLoginResponseAsync(ApplicationUser user, string? ip, string? userAgent)
        {
            var (accessToken, expiresAt, _) = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id, ip, userAgent);
            var roles = await _userManager.GetRolesAsync(user);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = expiresAt,
                UserId = user.Id,
                Email = user.Email ?? "",
                Name = user.Name,
                Roles = roles.ToList()
            };
        }
    }
}
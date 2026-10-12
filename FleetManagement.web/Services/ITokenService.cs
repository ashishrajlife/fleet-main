using FleetManagement.Web.Models;

namespace FleetManagement.Web.Services
{
    public interface ITokenService
    {
        Task<(string token, DateTime expiresAt, string jti)> GenerateAccessTokenAsync(ApplicationUser user);
        Task<string> GenerateRefreshTokenAsync(string userId, string? ip, string? userAgent);
        Task<(ApplicationUser user, RefreshToken token)?> ValidateRefreshTokenAsync(string rawToken);
        Task RevokeRefreshTokenAsync(RefreshToken token);
        Task BlacklistJtiAsync(string jti, string? userId, DateTime expiresAt, string? reason);
    }
}
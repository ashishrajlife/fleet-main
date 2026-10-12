using FleetManagement.Web.Models;

namespace FleetManagement.Web.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<LoginResponse>> LoginAsync(LoginRequest req, string? ip, string? userAgent);
        Task<ServiceResult<LoginResponse>> RefreshAsync(string rawRefreshToken, string? ip, string? userAgent);
        Task<ServiceResult> LogoutAsync(string? rawRefreshToken, string? jti, string? userId, DateTime? accessTokenExpiry);
        Task<ServiceResult<LoginResponse>> MeAsync(string userId);
    }
}
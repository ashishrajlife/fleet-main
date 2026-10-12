using FleetManagement.Web.Data;
using FleetManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FleetManagement.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration config, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _db = db;
            _userManager = userManager;
        }

        public async Task<(string token, DateTime expiresAt, string jti)> GenerateAccessTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var jti = Guid.NewGuid().ToString();
            var minutes = int.Parse(_config["Jwt:AccessTokenMinutes"] ?? "15");
            var expiresAt = DateTime.UtcNow.AddMinutes(minutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(JwtRegisteredClaimNames.Jti, jti),
                new("uid", user.Id),
                new("name", user.Name ?? "")
            };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt, jti);
        }

        public async Task<string> GenerateRefreshTokenAsync(string userId, string? ip, string? userAgent)
        {
            var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var hash = Sha256(raw);
            var jti = Guid.NewGuid().ToString();
            var days = int.Parse(_config["Jwt:RefreshTokenDays"] ?? "7");

            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = userId,
                TokenHash = hash,
                Jti = jti,
                ExpiresAt = DateTime.UtcNow.AddDays(days),
                IpAddress = ip,
                UserAgent = userAgent,
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();

            return raw;
        }

        public async Task<(ApplicationUser user, RefreshToken token)?> ValidateRefreshTokenAsync(string rawToken)
        {
            var hash = Sha256(rawToken);
            var token = await _db.RefreshTokens
                .FirstOrDefaultAsync(t => t.TokenHash == hash && t.RevokedAt == null);

            if (token == null || token.ExpiresAt < DateTime.UtcNow)
                return null;

            var user = await _userManager.FindByIdAsync(token.UserId);
            if (user == null) return null;

            return (user, token);
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken token)
        {
            token.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task BlacklistJtiAsync(string jti, string? userId, DateTime expiresAt, string? reason)
        {
            _db.TokenBlacklist.Add(new TokenBlacklist
            {
                Jti = jti,
                UserId = userId,
                ExpiresAt = expiresAt,
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }

        public static string Sha256(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
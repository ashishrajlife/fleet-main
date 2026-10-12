using System.ComponentModel.DataAnnotations;

namespace FleetManagement.Web.Models
{
    public class LoginRequest
    {
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required] public string Password { get; set; } = "";
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime AccessTokenExpiresAt { get; set; }
        public string UserId { get; set; } = "";
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public List<string> Roles { get; set; } = new();
    }

    public class RefreshRequest
    {
        [Required] public string RefreshToken { get; set; } = "";
    }

    public class LogoutRequest
    {
        [Required] public string RefreshToken { get; set; } = "";
    }
}
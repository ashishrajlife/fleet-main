using System.ComponentModel.DataAnnotations;

namespace FleetManagement.Web.Models
{
    // ---------- Input DTOs ----------
    public class CreateUserRequest
    {
        [Required] public string Name { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required] public string Phone { get; set; } = "";
        [Required, MinLength(8)] public string Password { get; set; } = "";
        [Required] public string Role { get; set; } = "driver";
    }

    public class UpdateUserRequest
    {
        [Required] public string Id { get; set; } = "";
        [Required] public string Name { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        public string? Phone { get; set; }
        [Required] public string Role { get; set; } = "viewer";
        public bool IsActive { get; set; } = true;
    }

    public class ResetPasswordRequest
    {
        [Required] public string UserId { get; set; } = "";
        [Required, MinLength(8)] public string NewPassword { get; set; } = "";
        [Required, Compare(nameof(NewPassword))] public string ConfirmPassword { get; set; } = "";
    }

    // ---------- Output DTO ----------
    public class UserResponse
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new();
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ---------- Result wrapper ----------
    public class ServiceResult
    {
        public bool Succeeded { get; init; }
        public List<string> Errors { get; init; } = new();
        public static ServiceResult Ok() => new() { Succeeded = true };
        public static ServiceResult Fail(params string[] errors) => new() { Succeeded = false, Errors = errors.ToList() };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; init; }
        public static ServiceResult<T> Ok(T data) => new() { Succeeded = true, Data = data };
        public static new ServiceResult<T> Fail(params string[] errors) => new() { Succeeded = false, Errors = errors.ToList() };
    }

    public class UpdateProfileRequest
    {
        [Required] public string Id { get; set; } = "";
        [Required] public string Name { get; set; } = "";
        public string? Phone { get; set; }
    }
}
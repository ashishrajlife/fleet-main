using FleetManagement.Web.Models;

namespace FleetManagement.Web.Services
{
    public interface IUserService
    {
        Task<ServiceResult<List<UserResponse>>> GetAllAsync();
        Task<ServiceResult<UserResponse>> GetByIdAsync(string id);
        Task<ServiceResult<UserResponse>> CreateAsync(CreateUserRequest req);
        Task<ServiceResult<UserResponse>> UpdateAsync(UpdateUserRequest req, string actorEmail);
        Task<ServiceResult> ToggleActiveAsync(string id, string actorEmail);
        Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest req);
        Task<ServiceResult> DeleteAsync(string id, string actorEmail);

        // Manager ke liye scoped list
        Task<ServiceResult<List<UserResponse>>> GetForManagerAsync(string actorEmail);
    }
}
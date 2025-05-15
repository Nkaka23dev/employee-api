using TheEmployeeAPI.Domain.DTOs.Users;

namespace TheEmployeeAPI.Application.User.Services
{
    public interface IUserService
    {
        Task<CurrentUserResponse> GetCurrentUser();
        Task<UserResponse> GetUserById(Guid id);
        Task<UserResponse> UpdateUser(Guid id, UpdatedUserRequest request);
        Task DeleteUser(Guid id);
    }
}

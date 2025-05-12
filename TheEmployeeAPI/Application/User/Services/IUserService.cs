using TheEmployeeAPI.Contracts.User;

namespace TheEmployeeAPI.Services.User
{
    public interface IUserServices
    {
        Task<CurrentUserResponse> GetCurrentUser();
        Task<UserResponse> GetUserById(Guid id);
        Task<UserResponse> UpdateUser(Guid id, UpdatedUserRequest request);
        Task DeleteUser(Guid id);
    }
}

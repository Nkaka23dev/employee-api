using TheEmployeeAPI.Domain.Contracts.Auth;

namespace TheEmployeeAPI.Services.User;

public interface IUserServices
{
  Task<UserResponse> RegisterHandler(UserRegisterRequest request);
  Task<UserResponse> LoginHandler(UserLoginRequest request);
  Task<CurrentUserResponse> GetCurrentUser();
  Task<UserResponse> GetUserById(Guid id);
  Task<UserResponse> UpdateUser(Guid id, UpdatedUserRequest request);
  Task DeleteUser(Guid id);
  Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest request);
  Task<CurrentUserResponse> RefreshAccessToken(RefreshTokenRequest request);
}

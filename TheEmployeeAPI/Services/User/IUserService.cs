using TheEmployeeAPI.Domain.Contracts.Auth;

namespace TheEmployeeAPI.Services.User;

public interface IUserService
{
  Task<UserResponse> RegisterHandler(UserRegisterRequest request);
  Task<UserResponse> LoginHandler(UserLoginRequest userLoginRequest);
  Task<CurrentUserResponse> GetCurrentUser();
  Task<UserResponse> GetUserById(Guid id);
  Task<UserResponse> UpdateUser(Guid id, UpdatedUserRequest updatedUserRequest);
  Task DeleteUser(Guid id);
  Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest refreshTokenRemoveRequest);
  Task<CurrentUserResponse> RefreshUserToken(RefreshTokenRequest refreshTokenRequest);
}

using TheEmployeeAPI.Contracts.Auth;
using TheEmployeeAPI.Contracts.User;

namespace TheEmployeeAPI.Services.Auth;

public interface IAuthService
{
    Task<UserResponse> RegisterHandler(UserRegisterRequest request);
    Task<UserResponse> LoginHandler(UserLoginRequest request);
    Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest request);
    Task<CurrentUserResponse> RefreshAccessToken(RefreshTokenRequest request);
}

using TheEmployeeAPI.Contracts.Auth;
using TheEmployeeAPI.Contracts.User;

namespace TheEmployeeAPI.Application.Authentication.Services
{
    public interface IAuthService
    {
        Task<UserResponse> RegisterHandler(RegisterRequest request);
        Task<UserResponse> LoginHandler(LoginRequest request);
        Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest request);
        Task<CurrentUserResponse> RefreshAccessToken(RefreshTokenRequest request);
    }
}

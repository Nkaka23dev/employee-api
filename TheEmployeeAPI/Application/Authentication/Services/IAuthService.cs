using TheEmployeeAPI.Contracts.Auth;
using TheEmployeeAPI.Contracts.User;

namespace TheEmployeeAPI.Application.Authentication.Services
{
    public interface IAuthService
    {
        Task<UserResponse> RegisterAsync(RegisterRequest request);
        Task<UserResponse> LoginAsync(LoginRequest request);
        Task<RevokeRefreshTokenResponse> RevokeRefreshTokenAsync(RefreshTokenRequest request);
        Task<CurrentUserResponse> RefreshAccessTokenAsync(RefreshTokenRequest request);
    }
}

using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Contracts.User;
using TheEmployeeAPI.Contracts.Auth;
using TheEmployeeAPI.Domain;
using TheEmployeeAPI.Application.Authentication.Services;

namespace TheEmployeeAPI.Services.Auth
{
    public class AuthService(
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    ILogger<AuthService> logger,
    ITokenService tokenService) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<UserResponse> RegisterHandler(RegisterRequest request)
        {
            _logger.LogInformation("Registering user..");

            var existingUser = await _userManager.FindByEmailAsync(request.Email ?? string.Empty);

            if (existingUser != null)
            {
                _logger.LogError("Email Already Exist");
                throw new BadHttpRequestException("Email Already Exist");
            }
            var newUser = _mapper.Map<ApplicationUser>(request);
            newUser.UserName = GenerateUniqueUserName(request?.FirstName!, request?.LastName!);
            var result = await _userManager.CreateAsync(newUser, request?.Password!);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create User: {errors}", errors);
                throw new Exception($"Failed to create User: {errors}");
            }
            _logger.LogInformation("User created successfully.");
            await _tokenService.GenerateToken(newUser);
            return _mapper.Map<UserResponse>(newUser);
        }
        //Login method
        public async Task<UserResponse> LoginHandler(LoginRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Login Request is null!");
                throw new ArgumentNullException(nameof(request));
            }
            var user = await _userManager.FindByEmailAsync(request?.Email!);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request?.Password!))
            {
                _logger.LogInformation("Invalid email or password");
                throw new Exception("Invalid Email or password!");
            }
            var accessToken = await _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));

            user.RefreshToken = Convert.ToBase64String(refreshTokenHash);
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(2);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to update user: {errors}", errors);
                throw new Exception($"Failed to updated the user: {errors}");
            }
            var userResponse = _mapper.Map<ApplicationUser, UserResponse>(user);
            userResponse.AccessToken = accessToken;
            userResponse.RefreshToken = refreshToken;

            return userResponse;
        }
        public async Task<CurrentUserResponse> RefreshAccessToken(RefreshTokenRequest request)
        {
            _logger.LogInformation("Refreshing access token using refresh token.");
            var refreshTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshToken!));
            var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);

            if (user == null)
            {
                _logger.LogInformation("Invalid Refresh Token");
                throw new Exception("Invalid Refresh Token");
            }
            if (user.RefreshTokenExpiryTime < DateTime.Now)
            {
                _logger.LogWarning("Refresh token is expired for user with user Id {userId}", user.Id);
                throw new Exception($"Refresh token is expired for user with user Id {user.Id}");
            }
            var newAccessToken = await _tokenService.GenerateToken(user);
            _logger.LogInformation("New Access Token Generated successfully");
            var currentUserResponse = _mapper.Map<CurrentUserResponse>(user);
            currentUserResponse.AccessToken = newAccessToken;
            return currentUserResponse;
        }
        public async Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest request)
        {
            _logger.LogInformation("Revoking Refresh token....");
            try
            {
                var refreshTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshToken!));
                var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);

                if (user == null)
                {
                    _logger.LogInformation("Invalid Refresh Token");
                    throw new Exception("Invalid Refresh Token");
                }
                if (user.RefreshTokenExpiryTime < DateTime.Now)
                {
                    _logger.LogWarning("Refresh token is expired for user with user Id {userId}", user.Id);
                    throw new Exception($"Refresh token is expired for user with user Id {user.Id}");
                }
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to update user");
                    return new RevokeRefreshTokenResponse
                    {
                        Message = "Failed to update refresh token"
                    };

                }
                _logger.LogInformation("Refresh token revoked successfully");
                return new RevokeRefreshTokenResponse
                {
                    Message = "Refresh token revoked successufully"
                };
            }
            catch (Exception exception)
            {
                _logger.LogInformation("Failed to revoke refresh token: {exception}", exception.Message);
                throw new Exception("Failed to revoke refresh token");
            }
        }
        private string GenerateUniqueUserName(string lastName, string firstname)
        {

            var baseUserName = $"{firstname}{lastName}".ToLower();
            var userName = baseUserName;
            var count = 1;
            while (_userManager.Users.Any(u => u.UserName == userName))
            {
                userName = $"{baseUserName}{count}";
                count++;
            }
            return userName;
        }

    }
}

using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using TheEmployeeAPI.Persistance.Repositories;
using TheEmployeeAPI.Domain.Entities;
using TheEmployeeAPI.Domain.DTOs.Users;
using TheEmployeeAPI.Domain.DTOs.Authentication;
using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Infrastructure.DbContexts;
using TheEmployeeAPI.Domain.Enums;


namespace TheEmployeeAPI.Application.Authentication.Services
{
    public class AuthService(
    IMapper mapper,
    ILogger<AuthService> logger,
    ITokenService tokenService,
    IAuthRepository authRepository,
    IUserRespository userRepository,
    UserManager<ApplicationUser> userManager,
    AppDbContext dbContext,
    RoleManager<IdentityRole> roleManager) : IAuthService

    {
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IUserRespository _userRepository = userRepository;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            _logger.LogInformation("Registering user..");

            var existingUser = await _authRepository
                .GetUserByEmailAsync(request.Email ?? string.Empty);

            if (existingUser != null)
            {
                _logger.LogError("Email Already Exist");
                throw new BadHttpRequestException("Email Already Exist");
            }

            // Normalize the role here:
            var normalizedRole = UserRoles.All
                .FirstOrDefault(r => r.Equals(request.Role, StringComparison.OrdinalIgnoreCase));

            if (normalizedRole == null)
            {
                _logger.LogError("Invalid role: {Role}", request.Role);
                throw new BadHttpRequestException($"Invalid role. Allowed roles: {string.Join(", ", UserRoles.All)}");
            }

            // Replace the role on request with the normalized one
            request.Role = normalizedRole;

            var newUser = _mapper.Map<ApplicationUser>(request);
            newUser.UserName = await GenerateUniqueUserName(request?.FirstName!, request?.LastName!);

            var result = await _authRepository.CreateUserAsync(newUser, request!.Password);

            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create User: {errors}", errors);
                throw new Exception($"Failed to create User: {errors}");
            }

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                _logger.LogInformation("Creating missing role: {Role}", request.Role);
                await _roleManager.CreateAsync(new IdentityRole(request.Role));
            }

            var assignResult = await _userManager.AddToRoleAsync(newUser, request.Role);
            if (!assignResult.Succeeded)
            {
                var roleErrors = string.Join(", ", assignResult.Errors.Select(e => e.Description));
                _logger.LogError("Failed to assign role: {errors}", roleErrors);
                throw new Exception($"Failed to assign role: {roleErrors}");
            }

            _logger.LogInformation("User created successfully.");
            await _tokenService.GenerateToken(newUser);

            var roles = await _userManager.GetRolesAsync(newUser);
            var userResponse = _mapper.Map<UserResponse>(newUser);

            userResponse.Role = roles.FirstOrDefault() ?? UserRoleEnum.Employee.ToString();

            await transaction.CommitAsync();
            return userResponse;
        }

        //Login method
        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Login Request is null!");
                throw new ArgumentNullException(nameof(request));
            }
            var user = await _authRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !await _authRepository.CheckUserPasswordAsync(user, request.Password!))
            {
                _logger.LogInformation("Invalid email or password");
                throw new Exception("Invalid Email or password!");
            }
            var accessToken = await _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));

            user.RefreshToken = Convert.ToBase64String(refreshTokenHash);
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(2);

            var result = await _authRepository.UpdateUserAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to update user: {errors}", errors);
                throw new Exception($"Failed to updated the user: {errors}");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userResponse = _mapper.Map<ApplicationUser, UserResponse>(user);

            userResponse.AccessToken = accessToken;
            userResponse.RefreshToken = refreshToken;
            userResponse.Role = roles.FirstOrDefault() ?? UserRoleEnum.Employee.ToString();
            return userResponse;
        }
        public async Task<CurrentUserResponse> RefreshAccessTokenAsync(RefreshTokenRequest request)
        {
            _logger.LogInformation("Refreshing access token using refresh token.");
            var refreshTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshToken!));
            var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);
            var user = await _authRepository
            .GetUserByHashedRefreshTokenAsync(refreshTokenHash, hashedRefreshToken);

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
        public async Task<RevokeRefreshTokenResponse> RevokeRefreshTokenAsync(RefreshTokenRequest request)
        {
            _logger.LogInformation("Revoking Refresh token....");
            try
            {
                var refreshTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshToken!));
                var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);
                var user = await _authRepository
                .GetUserByHashedRefreshTokenAsync(refreshTokenHash, hashedRefreshToken);

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
                var result = await _authRepository.UpdateUserAsync(user);
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
        private async Task<string> GenerateUniqueUserName(string lastName, string firstname)
        {
            var baseUserName = $"{firstname}{lastName}".ToLower();
            var userName = baseUserName;
            var count = 1;
            while (await _userRepository.IsUserNameTakenAsync(userName))
            {
                userName = $"{baseUserName}{count}";
                count++;
            }
            return userName;
        }
    }
}

using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.Contracts.Auth;
using TheEmployeeAPI.Entities.Auth;

namespace TheEmployeeAPI.Services.User;

public class UserService(
  ITokenService tokenService,
  ICurrentUserService currentUserService, 
  UserManager<ApplicationUser> userManager,
  IMapper mapper,
  ILogger<UserService> logger) : IUserServices
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UserService> _logger = logger;
     
    //Register method
    public async Task<UserResponse> RegisterHandler(UserRegisterRequest request)
    {
        _logger.LogInformation("Registering user..");

        var existingUser = await _userManager.FindByEmailAsync(request.Email ?? string.Empty);

        if(existingUser != null){
            _logger.LogError("Email Already Exist");
            throw new BadHttpRequestException("Email Already Exist");
        }
        var newUser = _mapper.Map<ApplicationUser>(request);
        newUser.UserName = GenerateUniqueUserName(request?.FirstName!, request?.LastName!);
        var result = await _userManager.CreateAsync(newUser, request?.Password!);

        if(!result.Succeeded){
          var errors = string.Join(", ", result.Errors.Select(e => e.Description));
          _logger.LogError("Failed to create User: {errors}", errors);
          throw new Exception($"Failed to create User: {errors}");
        }
        _logger.LogInformation("User created successfully.");
        await _tokenService.GenerateToken(newUser);
        return _mapper.Map<UserResponse>(newUser);
    }  
     
    //Login method
    public async Task<UserResponse> LoginHandler(UserLoginRequest request)
    {
        if (request == null)
        {
            _logger.LogError("Login Request is null!");
            throw new ArgumentNullException(nameof(request));
        }
        var user = await _userManager.FindByEmailAsync(request?.Email!);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request?.password!))
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
    public async Task<UserResponse> GetUserById(Guid id)
    {
       _logger.LogInformation("Getting user by ID: ");
       var user = await _userManager.FindByIdAsync(id.ToString());
       if(user == null){
        _logger.LogError("User with id: {id} not found", id);
          throw new Exception($"User with Id {id} not found!");
       }
       return _mapper.Map<ApplicationUser, UserResponse>(user);
    }
    public async Task<CurrentUserResponse> GetCurrentUser()
    {
       var user = await _userManager.FindByIdAsync(_currentUserService.GetUserId());

        if(user == null){
        _logger.LogError("User  not found");
          throw new Exception($"User not found!");
       } 
       return _mapper.Map<CurrentUserResponse>(user);

    }
    public async  Task<CurrentUserResponse> RefreshAccessToken(RefreshTokenRequest request)
    {
        _logger.LogInformation("Refreshing access token using refresh token.");
        var refreshTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshToken!));
        var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);

        if(user == null){
            _logger.LogInformation("Invalid Refresh Token");
            throw new Exception("Invalid Refresh Token");
        }
        if(user.RefreshTokenExpiryTime < DateTime.Now){
            _logger.LogWarning("Refresh token is expired for user with user Id {userId}", user.Id);
            throw new Exception($"Refresh token is expired for user with user Id {user.Id}");
        }
        var newAccessToken = await _tokenService.GenerateToken(user);
        _logger.LogInformation("New Access Token Generated successfully");
        var currentUserResponse = _mapper.Map<CurrentUserResponse>(user);
        currentUserResponse.AccessToken = newAccessToken;
        return _mapper.Map<CurrentUserResponse>(newAccessToken);
    }
    public Task DeleteUser(Guid id)
    {
        throw new NotImplementedException();
    }
   
    public Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest refreshTokenRemoveRequest)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse> UpdateUser(Guid id, UpdatedUserRequest updatedUserRequest)
    {
        throw new NotImplementedException();
    }
    private string GenerateUniqueUserName(string lastName, string firstname){

     var baseUserName = $"{firstname}{lastName}".ToLower();
     var userName = baseUserName;
     var count = 1; 
     while(_userManager.Users.Any(u => u.UserName == userName)){
        userName = $"{baseUserName}{count}";
        count++;
     }
     return userName;
    } 
}

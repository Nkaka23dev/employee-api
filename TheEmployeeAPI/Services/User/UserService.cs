using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Domain.Contracts.Auth;
using TheEmployeeAPI.Entities.Auth;

namespace TheEmployeeAPI.Services.User;

public class UserService : IUserService
{
    private readonly ITokenService _tokenService;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(ITokenService tokenService, ICurrentUserService
     currentUserService, UserManager<ApplicationUser> userManager,
    IMapper mapper, ILogger<UserService> logger){
       _tokenService = tokenService;
       _currentUserService = currentUserService;
       _userManager = userManager;
       _mapper = mapper;
       _logger = logger;
    } 
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
    public Task DeleteUser(Guid   id)
    {
        throw new NotImplementedException();
    }

    public Task<CurrentUserResponse> GetCurrentUser()
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse> GetUserById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse> LoginHandler(UserLoginRequest userLoginRequest)
    {
        throw new NotImplementedException();
    }

    public Task<CurrentUserResponse> RefreshUserToken(RefreshTokenRequest refreshTokenRequest)
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
}

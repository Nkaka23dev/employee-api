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
    public  Task<UserResponse> RegisterHandler(UserRegisterRequest request)
    {
        // _logger.LogInformation("Registering user..");
        // var existingUser = await _userManager.FindByEmailAsync(request.Email ?? string.Empty);
        // if(existingUser != null){
        //     _logger.LogError("Email Already Exist");
        //     throw new BadHttpRequestException("Email Already Exist");
        // }
        // var newUser = _mapper.Map<ApplicationUser>(request);

        // //Generate a unique username
        
        // return '';
        throw new NotImplementedException();
    }  
    public Task DeleteUser(Guid id)
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

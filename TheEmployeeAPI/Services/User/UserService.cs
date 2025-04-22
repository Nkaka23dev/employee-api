using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Contracts.User;
using TheEmployeeAPI.Entities.Auth;

namespace TheEmployeeAPI.Services.User;

public class UserService(
  ICurrentUserService currentUserService, 
  UserManager<ApplicationUser> userManager,
  IMapper mapper,
  ILogger<UserService> logger) : IUserServices
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UserService> _logger = logger;
 
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
    public async Task<UserResponse> UpdateUser(Guid id, UpdatedUserRequest request)
    {
       var user = await _userManager.FindByIdAsync(id.ToString());
       if(user == null){
        _logger.LogError("User not found!");
        throw new Exception("User not found!");
       } 
       user.FirstName = request.FirstName!;
       user.LastName = request.LastName!;
       user.Email = request.Email;
       user.Gender = request.Gender!;
       await _userManager.UpdateAsync(user);
       return _mapper.Map<UserResponse>(user);
    }
     public async Task DeleteUser(Guid id)
    {
       var user = await _userManager.FindByIdAsync(id.ToString());
       if(user == null){
        _logger.LogError("User not found!");
        throw new Exception("User not found!");
       } 
       await _userManager.DeleteAsync(user);
    }
}

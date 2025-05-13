using AutoMapper;
using TheEmployeeAPI.Contracts.User;
using TheEmployeeAPI.Domain;
using TheEmployeeAPI.Persistance.Repositories;
using TheEmployeeAPI.Services;
using TheEmployeeAPI.Services.User;

namespace TheEmployeeAPI.Application.User.Services
{
    public class UserService(
      ICurrentUserService currentUserService,
      IUserRespository userRespository,
      IMapper mapper,
      ILogger<UserService> logger) : IUserService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IUserRespository _userRespository = userRespository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserService> _logger = logger;

        public async Task<UserResponse> GetUserById(Guid id)
        {
            _logger.LogInformation("Getting user by ID: ");
            var user = await _userRespository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogError("User with id: {id} not found", id);
                throw new Exception($"User with Id {id} not found!");
            }
            return _mapper.Map<ApplicationUser, UserResponse>(user);
        }
        public async Task<CurrentUserResponse> GetCurrentUser()
        {
            var userId = Guid.Parse(_currentUserService.GetUserId());
            var user = await _userRespository.GetUserByIdAsync(userId);

            if (user == null)
            {
                _logger.LogError("User  not found");
                throw new Exception($"User not found!");
            }
            return _mapper.Map<CurrentUserResponse>(user);
        }
        public async Task<UserResponse> UpdateUser(Guid id, UpdatedUserRequest request)
        {
            var user = await _userRespository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogError("User not found!");
                throw new Exception("User not found!");
            }
            user.FirstName = request.FirstName!;
            user.LastName = request.LastName!;
            user.Email = request.Email;
            user.Gender = request.Gender!;
            await _userRespository.UpdateUserAsync(user);
            return _mapper.Map<UserResponse>(user);
        }
        public async Task DeleteUser(Guid id)
        {
            var user = await _userRespository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogError("User not found!");
                throw new Exception("User not found!");
            }
            await _userRespository.DeleteUserAsync(user);
        }
    }
}

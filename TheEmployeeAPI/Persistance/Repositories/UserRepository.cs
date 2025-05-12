using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain;
using TheEmployeeAPI.Persistance.Repositories.Users;

namespace TheEmployeeAPI.Persistance.Repositories;

public class UserRepository(
    UserManager<ApplicationUser> userManager
    ) : IUserRespository
{
    public UserManager<ApplicationUser> _userManager = userManager;

    public Task DeleteUserAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUser> GetCurrentUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUser> GetUserByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserNameTakenAsync(string userName)
    {
        return await _userManager.Users.AnyAsync(u => u.UserName == userName);
    }

    public Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
    {
        throw new NotImplementedException();
    }
}

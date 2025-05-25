using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Persistance.Repositories;

public class UserRepository(
    UserManager<ApplicationUser> userManager
    ) : IUserRespository
{
    public UserManager<ApplicationUser> _userManager = userManager;

    public Task DeleteUserAsync(ApplicationUser user)
    {
        return _userManager.DeleteAsync(user);
    }
    public async Task<ApplicationUser?> GetUserByIdAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }

    public async Task<bool> IsUserNameTakenAsync(string userName)
    {
        return await _userManager.Users.AnyAsync(u => u.UserName == userName);
    }

    public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
    {
        return await _userManager.UpdateAsync(user);
    }

}

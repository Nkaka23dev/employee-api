using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Persistance.Repositories;

public interface IUserRespository
{
    Task<ApplicationUser?> GetUserByIdAsync(Guid id);
    Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
    Task DeleteUserAsync(ApplicationUser user);
    Task<bool> IsUserNameTakenAsync(string userName);
}

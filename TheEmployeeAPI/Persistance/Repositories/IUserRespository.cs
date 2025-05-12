using TheEmployeeAPI.Domain;

namespace TheEmployeeAPI.Persistance.Repositories.Users;

public interface IUserRespository
{
    Task<ApplicationUser> GetUserByIdAsync(Guid id);
    Task<ApplicationUser> GetCurrentUserAsync();
    Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
    Task DeleteUserAsync(Guid id);
    Task<bool> IsUserNameTakenAsync(string userName);
}

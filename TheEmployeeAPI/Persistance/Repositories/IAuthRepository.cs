using TheEmployeeAPI.Domain;

namespace TheEmployeeAPI.Persistance.Repositories.Authentication;

public interface IAuthRepository
{
    Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<bool> ClearRefreshTokenAsync(ApplicationUser user);
    Task<ApplicationUser?> GetByHashedRefreshTokenAsync(string hashedRefreshToken);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
}

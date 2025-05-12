using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Domain;

namespace TheEmployeeAPI.Persistance.Repositories;

public interface IAuthRepository
{
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
    Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password);
    Task<ApplicationUser?> GetUserByHashedRefreshTokenAsync(byte[] refreshToken, string hashedRefreshToken);
    Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
    Task<ApplicationUser?> GetUserByEmailAsync(string? email);
    Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
}

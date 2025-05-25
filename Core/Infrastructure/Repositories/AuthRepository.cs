using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.Entities;


namespace Core.Infrastructure.Repositories
{
    public class AuthRepository(
        UserManager<ApplicationUser> userManager
        ) : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password!);
        }
        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUser?>
        GetUserByHashedRefreshTokenAsync(byte[] refreshToken, string hashedRefreshToken)
        {
            return await _userManager
            .Users
            .FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);
        }
        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string? email)
        {
            return await _userManager.FindByEmailAsync(email ?? string.Empty);
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}

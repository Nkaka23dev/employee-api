
using TheEmployeeAPI.Domain.Authentication;

namespace TheEmployeeAPI.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
        string GenerateRefreshToken();

    }
}

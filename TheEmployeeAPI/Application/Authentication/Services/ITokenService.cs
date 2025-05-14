using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Application.Authentication.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
        string GenerateRefreshToken();

    }
}

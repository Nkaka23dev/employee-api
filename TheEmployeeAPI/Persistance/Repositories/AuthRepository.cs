using TheEmployeeAPI.Domain;

namespace TheEmployeeAPI.Persistance.Repositories.Authentication;

public class AuthRepository : IAuthRepository
{
    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ClearRefreshTokenAsync(ApplicationUser user)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUser> CreateAsync(ApplicationUser user, string password)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUser?> GetByHashedRefreshTokenAsync(string hashedRefreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {
        throw new NotImplementedException();
    }
}

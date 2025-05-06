using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TheEmployeeAPI.Domain.Contracts;
using TheEmployeeAPI.Entities.Auth;

namespace TheEmployeeAPI.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _secretKey;
    private readonly string? _validIssuer;
    private readonly string? _validAudience;
    private readonly double? _expires;
    private readonly UserManager<ApplicationUser> _userManager;
    // private readonly ILogger<TokenService> _logger;
    public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
        {
            throw new InvalidOperationException("JWT secret key is not configured");
        }
        _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        _userManager = userManager;
        _validIssuer = jwtSettings.ValidIssuer;
        _expires = jwtSettings.Expires;
        _validAudience = jwtSettings.ValidAudience;
    }
    public async Task<string> GenerateToken(ApplicationUser user)
    {
        var signingCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);
        return refreshToken;
    }
    private async Task<List<Claim>> GetClaims(ApplicationUser user)
    {
        var claims = new List<Claim>{
    new(ClaimTypes.Name, user?.UserName ?? string.Empty),
    new(ClaimTypes.NameIdentifier, user?.Id ?? string.Empty),
    new(ClaimTypes.Email, user?.Email ?? string.Empty),
    new("FirstName", user?.FirstName ?? string.Empty),
    new("LastName", user?.LastName ?? string.Empty),
    new("Gender", user?.Gender ?? string.Empty)
    };
        var roles = await _userManager.GetRolesAsync(user!);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        return new JwtSecurityToken(
          issuer: _validIssuer,
          audience: _validAudience,
          claims: claims,
          expires: DateTime.Now.AddMinutes(_expires ?? 120),
          signingCredentials: signingCredentials
        );
    }

}

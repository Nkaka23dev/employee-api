using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Domain.Entities;
using TheEmployeeAPI.Infrastructure.DbContexts;

namespace TheEmployeeAPI.WebAPI.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;

        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}

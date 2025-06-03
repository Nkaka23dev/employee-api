using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.Entities;
using TheEmployeeAPI.Infrastructure.DbContexts;
using TheEmployeeAPI.Infrastructure.Seed;

namespace TheEmployeeAPI.WebAPI.Extensions;

public static class DatabaseSetupExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        var config = app.Configuration;
        var seedMode = config.GetValue<string>("SeedMode")?.ToLowerInvariant();

        if (seedMode is null or "none")
        {
            app.Logger.LogInformation("SeedMode is 'None' – skipping DB setup.");
            return;
        }

        app.Logger.LogInformation("SeedMode is '{SeedMode}' – initializing database.", seedMode);

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        switch (seedMode)
        {
            case "reset":
                app.Logger.LogInformation("Resetting database...");
                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();
                break;

            case "migrate":
                app.Logger.LogInformation("Applying migrations...");
                await dbContext.Database.MigrateAsync();
                break;

            default:
                app.Logger.LogWarning("Invalid SeedMode: '{SeedMode}' – skipping DB setup.", seedMode);
                return;
        }

        await app.SeedDatabase();
    }

    public static async Task SeedDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        await UsersSeeder.SeedUsersAsync(userManager, roleManager);
        EmployeeSeeder.EmployeesSeeder(dbContext);
        BenefitsSeeder.SeedBenefits(dbContext);
        EmployeeSeeder.AssignEmployeeBenefits(dbContext);
    }
}

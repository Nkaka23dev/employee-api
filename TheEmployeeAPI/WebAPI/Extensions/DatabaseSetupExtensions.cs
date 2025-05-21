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
        if (!app.Environment.IsDevelopment())
            return;

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync();
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



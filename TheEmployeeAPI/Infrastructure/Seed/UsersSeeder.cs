using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Infrastructure.Seed;

public static class UsersSeeder
{
    public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var users = new List<(string FirstName, string LastName, string Email, string Password, string Gender, string Role)>
        {
            ("Eric", "Nkaka", "eric.nkaka@example.com", "Password123!", "Male", "Employee"),
            ("Jane", "Smith", "jane.smith@example.com", "SecurePass456!", "Female", "Employee")
        };

        foreach (var (firstName, lastName, email, password, gender, role) in users)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                // Ensure the role exists
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        throw new Exception($"Failed to create role '{role}': {errors}");
                    }
                }

                var user = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    Gender = gender,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user {email}: {errors}");
                }

                // Assign role
                var assignResult = await userManager.AddToRoleAsync(user, role);
                if (!assignResult.Succeeded)
                {
                    var errors = string.Join(", ", assignResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to assign role '{role}' to user {email}: {errors}");
                }
            }
        }
    }
}

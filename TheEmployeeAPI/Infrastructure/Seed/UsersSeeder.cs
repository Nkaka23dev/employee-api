using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Infrastructure.Seed;

public static class UsersSeeder
{
    public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
    {
        var users = new List<(string FirstName, string LastName, string Email, string Password, string Gender)>
        {
            ("Eric", "Nkaka", "eric.nkaka@example.com", "Password123!", "Male"),
            ("Jane", "Smith", "jane.smith@example.com", "SecurePass456!", "Female")
        };

        foreach (var (firstName, lastName, email, password, gender) in users)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    Gender = gender,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user {email}: {errors}");
                }
            }
        }
    }
}

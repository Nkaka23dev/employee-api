using TheEmployeeAPI.Domain.Entities;
using TheEmployeeAPI.Infrastructure.DbContexts;

namespace TheEmployeeAPI.Infrastructure.Seed;

public class BenefitsSeeder
{
    public static void SeedBenefits(AppDbContext context)
    {
        var existingBenefitNames = context.Benefits.Select(b => b.Name).ToHashSet();

        var newBenefits = new List<Benefit>
    {
        new() { Name = "Health", Description = "...", BaseCost = 100.00m },
        new() { Name = "Dental", Description = "...", BaseCost = 50.00m },
        new() { Name = "Vision", Description = "...", BaseCost = 30.00m }
    }.Where(b => !existingBenefitNames.Contains(b.Name));

        if (newBenefits.Any())
        {
            context.Benefits.AddRange(newBenefits);
            context.SaveChanges();
        }
    }

}
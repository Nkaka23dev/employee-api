using PayrollService.Models;

namespace PayrollService.Data;

public static class PrepDb
{
    public static void PrepPopulation(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
        if (context != null)
        {
            SeedData(context);
        }

    }
    private static void SeedData(AppDbContext context)
    {
        if (!context.Payrolls.Any())
        {
            Console.WriteLine("----> Seeding data started.......");
            var payrolls = new List<Payroll>
            {
                new() {
                    WorkLocation = "Kigali",
                    Notes = "Monthly salary for June",
                    ProjectCode = "PRJ-2025-01",
                    Amount = "500000"
                },
                new() {
                    WorkLocation = "Huye",
                    Notes = "Contractor payment",
                    ProjectCode = null,
                    Amount = "300000"
                },
                new() {
                    WorkLocation = "Remote",
                    Notes = "Freelancer payment",
                    ProjectCode = "PRJ-2025-03",
                    Amount = "450000"
                }
            };

            context.Payrolls.AddRange(
               payrolls
            );
            context.SaveChanges();

        }
        else
        {
            Console.WriteLine("----> We already have data");
        }
    }
}

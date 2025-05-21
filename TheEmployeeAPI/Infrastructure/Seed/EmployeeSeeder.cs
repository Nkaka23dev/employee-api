using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.Entities;
using TheEmployeeAPI.Infrastructure.DbContexts;

namespace TheEmployeeAPI.Infrastructure.Seed;

public static class EmployeeSeeder
{
    public static void EmployeesSeeder(AppDbContext context)
    {
        var existingEmployeeEmails = context.Employees.Select(e => e.Email).ToHashSet();

        var newEmployees = new List<Employee>
    {
        new() {
            FirstName = "John",
            LastName = "Doe",
            SocialSecurityNumber = "123-45-6789",
            Address1 = "123 Main St",
            City = "Anytown",
            State = "NY",
            ZipCode = "12345",
            PhoneNumber = "555-123-4567",
            Email = "john.doe@example.com"
        },
        new()
        {
            FirstName = "Jane",
            LastName = "Smith",
            SocialSecurityNumber = "987-65-4321",
            Address1 = "456 Elm St",
            Address2 = "Apt 2B",
            City = "Othertown",
            State = "CA",
            ZipCode = "98765",
            PhoneNumber = "555-987-6543",
            Email = "jane.smith@example.com"
        }
    }.Where(e => !existingEmployeeEmails.Contains(e.Email));

        if (newEmployees.Any())
        {
            context.Employees.AddRange(newEmployees);
            context.SaveChanges();
        }
    }

    public static void AssignEmployeeBenefits(AppDbContext context)
    {
        var john = context.Employees.Include(e => e.Benefits).FirstOrDefault(e => e.FirstName == "John");
        var jane = context.Employees.Include(e => e.Benefits).FirstOrDefault(e => e.FirstName == "Jane");
        var health = context.Benefits.FirstOrDefault(b => b.Name == "Health");
        var dental = context.Benefits.FirstOrDefault(b => b.Name == "Dental");
        var vision = context.Benefits.FirstOrDefault(b => b.Name == "Vision");

        if (john != null)
        {
            if (!john.Benefits.Any(b => b.BenefitId == health?.Id))
            {
                john.Benefits.Add(new EmployeeBenefit { Benefit = health!, CostToEmployee = 100m });
            }
            if (!john.Benefits.Any(b => b.BenefitId == dental?.Id))
            {
                john.Benefits.Add(new EmployeeBenefit { Benefit = dental! });
            }
        }

        if (jane != null)
        {
            if (!jane.Benefits.Any(b => b.BenefitId == health?.Id))
            {
                jane.Benefits.Add(new EmployeeBenefit { Benefit = health!, CostToEmployee = 120m });
            }
            if (!jane.Benefits.Any(b => b.BenefitId == vision?.Id))
            {
                jane.Benefits.Add(new EmployeeBenefit { Benefit = vision! });
            }
        }

        context.SaveChanges();
    }

}

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
        },
         new()
    {
        FirstName = "Eric",
        LastName = "Ndayisaba",
        SocialSecurityNumber = "RW123456789",
        Address1 = "KN 3 Ave",
        City = "Kigali",
        State = "Gasabo",
        ZipCode = "00100",
        PhoneNumber = "+250-788-123-456",
        Email = "eric.ndayisaba@example.rw"
    },
    new()
    {
        FirstName = "Aline",
        LastName = "Uwimana",
        SocialSecurityNumber = "RW987654321",
        Address1 = "KG 11 Ave",
        Address2 = "House 25",
        City = "Kigali",
        State = "Kicukiro",
        ZipCode = "00200",
        PhoneNumber = "+250-722-456-789",
        Email = "aline.uwimana@example.rw"
    },
    new()
    {
        FirstName = "Jean Claude",
        LastName = "Hategekimana",
        SocialSecurityNumber = "RW456123789",
        Address1 = "RN1 Road",
        City = "Huye",
        State = "Southern",
        ZipCode = "00300",
        PhoneNumber = "+250-789-321-654",
        Email = "jchategekimana@example.rw"
    },
    new()
    {
        FirstName = "Clarisse",
        LastName = "Ingabire",
        SocialSecurityNumber = "RW321654987",
        Address1 = "Gikondo Industrial Zone",
        City = "Kigali",
        State = "Kicukiro",
        ZipCode = "00400",
        PhoneNumber = "+250-730-987-321",
        Email = "clarisse.ingabire@example.rw"
    },
    new()
    {
        FirstName = "Patrick",
        LastName = "Mugisha",
        SocialSecurityNumber = "RW654789321",
        Address1 = "Nyagatare Town",
        City = "Nyagatare",
        State = "Eastern",
        ZipCode = "00500",
        PhoneNumber = "+250-788-456-123",
        Email = "patrick.mugisha@example.rw"
    },
    new()
    {
        FirstName = "Diane",
        LastName = "Mukamana",
        SocialSecurityNumber = "RW789321456",
        Address1 = "Kabuye Hill",
        City = "Rubavu",
        State = "Western",
        ZipCode = "00600",
        PhoneNumber = "+250-721-654-987",
        Email = "diane.mukamana@example.rw"
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

namespace TheEmployeeAPI;

public class SeedData
{
 public static void Seed(IServiceProvider serviceProvide){
    var context = serviceProvide.GetRequiredService<AppBbContext>();
    if(!context.Employees.Any()){
        context.Employees.AddRange(
        new Employee
        {
            FirstName = "John",
            LastName = "Doe",
            SocialSecurityNumber = "123-45-6789",
            Address1 = "123 Main St",
            City = "Anytown",
            State = "NY",
            ZipCode = "12345",
            PhoneNumber = "555-123-4567",
            Email = "john.doe@example.com",
            Benefits = new List<EmployeeBenefits>
            {
                new EmployeeBenefits { BenefitsType = BenefitsType.Health, Cost = 100.00m },
                new EmployeeBenefits { BenefitsType = BenefitsType.Dental, Cost = 50.00m }
            }
        },
        new Employee
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
            Email = "jane.smith@example.com",
            Benefits = new List<EmployeeBenefits>
            {
                new EmployeeBenefits { BenefitsType = BenefitsType.Health, Cost = 120.00m },
                new EmployeeBenefits { BenefitsType = BenefitsType.Vision, Cost = 30.00m }
            }
        }
        );
    }

 }
}
 
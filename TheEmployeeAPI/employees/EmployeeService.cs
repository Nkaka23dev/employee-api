namespace TheEmployeeAPI.employees;

public class EmployeeService
{
public static GetEmployeeResponseEmployeeBenefits BenefitsToBenefitsResponse(EmployeeBenefits employeeBenefits){
    return new GetEmployeeResponseEmployeeBenefits {
    Id = employeeBenefits.Id,
    EmployeeId = employeeBenefits.EmployeeId,
    BenefitsType = employeeBenefits.BenefitsType,
    Cost = employeeBenefits.Cost
    };
    }

public static GetEmployeeResponse EmployeeToGetEmployeeResponse(Employee employee){
    return new GetEmployeeResponse {
        FirstName = employee.FirstName, 
        LastName = employee.LastName,
        Address1 = employee.Address1,
        Address2 = employee.Address2,
        City = employee.City,
        Email = employee.Email,
        ZipCode = employee.ZipCode,
        PhoneNumber = employee.PhoneNumber,     
        State = employee.State, 
        Benefits = employee.Benefits.Select(benefit => new GetEmployeeResponseEmployeeBenefits{
            Id = benefit.Id,
            EmployeeId = benefit.EmployeeId,
            BenefitsType = benefit.BenefitsType,
            Cost = benefit.Cost
        }).ToList()
    };
    }
}

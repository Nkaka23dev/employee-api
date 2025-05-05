using TheEmployeeAPI.Contracts.Employee;
using TheEmployeeAPI.Entities.Employee;

namespace TheEmployeeAPI.Services.Employees;

public interface IEmployeeService
{
    Task<IEnumerable<GetEmployeeResponse>> GetAllEmployeesAsync(GetAllEmployeesRequest request);
    Task<GetEmployeeResponse> GetEmployeeAsync(int id);
    Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest request);
    Task<GetEmployeeResponse> UpdateEmployeeAsync(UpdateEmployeeRequest request, int id);
    Task DeleteEmployeeAsync(int id);
    Task<GetEmployeeResponseEmployeeBenefits> GetBenefitsForEmployeeAsync(int employeeId);
}

using TheEmployeeAPI.Contracts.Employee;

namespace TheEmployeeAPI.Services.Employees;

public interface IEmployeeService
{
  Task<IEnumerable<GetEmployeeResponse>> GetAllEmployeesAsync(GetAllEmployeesRequest request);
}

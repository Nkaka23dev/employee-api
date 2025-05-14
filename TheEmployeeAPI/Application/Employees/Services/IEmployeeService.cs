using TheEmployeeAPI.Application.Employees.DTOs;
using TheEmployeeAPI.Contracts.Employee;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Application.Employees.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<GetEmployeeResponse>> GetAllEmployeesAsync(GetAllEmployeesRequest request);
        Task<GetEmployeeResponse> GetEmployeeAsync(int id);
        Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest request);
        Task<GetEmployeeResponse> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request);
        Task DeleteEmployeeAsync(int id);
        Task<IEnumerable<GetEmployeeResponseEmployeeBenefits>> GetBenefitsForEmployeeAsync(int employeeId);
    }
}

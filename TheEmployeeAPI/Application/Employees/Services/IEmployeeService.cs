using TheEmployeeAPI.Domain.DTOs.Employees;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Application.Employees.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<GetEmployeeResponse>> GetAllEmployeesAsync(GetAllEmployeesRequest request);
        Task<GetEmployeeResponse> GetEmployeeAsync(int id);
        Task<GetEmployeeResponse> CreateEmployeeAsync(CreateEmployeeRequest request);
        Task<GetEmployeeResponse> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request);
        Task DeleteEmployeeAsync(int id);
        Task<IEnumerable<GetEmployeeResponseEmployeeBenefits>> GetBenefitsForEmployeeAsync(int employeeId);
    }
}

using TheEmployeeAPI.Entities.Employee;

namespace TheEmployeeAPI.Persistance.Repositories.Employees;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> DeleteAsync(int id);

}

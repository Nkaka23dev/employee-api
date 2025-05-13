using TheEmployeeAPI.Entities.Employee;

namespace TheEmployeeAPI.Persistance.Repositories.Employees;

public interface IEmployeeRepository
{
    IQueryable<Employee> GetQuery(int numberOfRecord, int page);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task<Employee?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task<Employee> GetBenefits(int id);

}

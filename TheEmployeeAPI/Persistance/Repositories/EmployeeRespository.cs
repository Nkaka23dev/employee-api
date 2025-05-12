using TheEmployeeAPI.Entities.Employee;
using TheEmployeeAPI.Persistance.Repositories.Employees;

namespace TheEmployeeAPI.Persistance.Repositories;

public class EmployeeRespository : IEmployeeRepository
{
    public Task<Employee> AddAsync(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task<Employee> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Employee>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Employee?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Employee> UpdateAsync(Employee employee)
    {
        throw new NotImplementedException();
    }
}

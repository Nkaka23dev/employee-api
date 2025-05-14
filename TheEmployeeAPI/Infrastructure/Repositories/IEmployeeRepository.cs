using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Persistance.Repositories;

public interface IEmployeeRepository
{
    IQueryable<Employee> GetQuery(int? numberOfRecord = null, int? page =  null);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task<Employee?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task<Employee> GetBenefits(int id);

}

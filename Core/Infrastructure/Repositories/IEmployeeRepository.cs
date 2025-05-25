using TheEmployeeAPI.Domain.Entities;

namespace Core.Infrastructure.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> GetBenefits(int id);

    }
}

using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.Entities;
using TheEmployeeAPI.Infrastructure.DbContexts;

namespace Core.Infrastructure.Repositories
{
    public class EmployeeRespository(AppDbContext dbContext) : IEmployeeRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task AddAsync(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id)
            ?? throw new Exception($"Employee with {id} not found!");
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }
        public IQueryable<Employee> GetQuery(int? page = null, int? numberOfRecord = null)
        {
            var query = _dbContext.Employees
                .Include(e => e.Benefits)
                .AsQueryable();

            if (page.HasValue && numberOfRecord.HasValue)
            {
                query = query
                    .Skip((page.Value - 1) * numberOfRecord.Value)
                    .Take(numberOfRecord.Value);
            }

            return query;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _dbContext.Employees.SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(Employee employee)
        {
            var existing = await _dbContext.Employees.FindAsync(employee.Id) ?? throw new Exception($"Employee with id {employee.Id} not found.");
            _dbContext.Entry(existing).CurrentValues.SetValues(employee);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<Employee> GetBenefits(int id)
        {
            return await _dbContext.Employees
                  .Include(e => e.Benefits)
                  .ThenInclude(e => e.Benefit)
                  .SingleOrDefaultAsync(e => e.Id == id) ??
                  throw new KeyNotFoundException($"Employee with {id} not found!");
        }
    }
}

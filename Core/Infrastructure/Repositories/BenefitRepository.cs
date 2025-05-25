using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.Entities;
using TheEmployeeAPI.Infrastructure.DbContexts;

namespace Core.Infrastructure.Repositories;

public class BenefitRepository(AppDbContext dbContext) : IRepository<Benefit>
{
    private readonly AppDbContext _dbContext = dbContext;
    public  async Task AddAsync(Benefit entity)
    {
        _dbContext.Benefits.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var employeBenefit = await _dbContext.Benefits.FindAsync(id) ??
        throw new Exception($"Benefit with {id} Not found!");
        _dbContext.Benefits.Remove(employeBenefit);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Benefit?> GetByIdAsync(int id)
    {
        return await _dbContext.Benefits.SingleOrDefaultAsync(e => e.Id == id);
    }

    public IQueryable<Benefit> GetQuery(int? numberOfRecord = null, int? page = null)
    {
        var query = _dbContext.Benefits.AsQueryable();
        return query;
    }

    public async Task UpdateAsync(Benefit entity)
{
    var existing = await _dbContext.Benefits.FindAsync(entity.Id) ?? throw new Exception($"Benefit with id {entity.Id} not found.");
        _dbContext.Entry(existing).CurrentValues.SetValues(entity);
    await _dbContext.SaveChangesAsync();
}
}

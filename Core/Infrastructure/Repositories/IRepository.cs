namespace Core.Infrastructure.Repositories;

public interface IRepository<T>  where T : class
{
    IQueryable<T> GetQuery(int? numberOfRecord = null, int? page = null);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}

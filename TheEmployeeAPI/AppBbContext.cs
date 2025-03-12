using Microsoft.EntityFrameworkCore;

namespace TheEmployeeAPI;

public class AppBbContext: DbContext
{
    public AppBbContext(DbContextOptions<AppBbContext> options): base(options){

    }
    public DbSet<Employee> Employees {get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
 
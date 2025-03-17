using Microsoft.EntityFrameworkCore;

namespace TheEmployeeAPI;

public class AppBbContext: DbContext
{
    public AppBbContext(DbContextOptions<AppBbContext> options): base(options){
 
    }
    public DbSet<Employee> Employees {get; set;}
    public DbSet<Benefit> Benefits {get; set;}
    public DbSet<EmployeeBenefit> EmployeeBenefits {get;set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeBenefit>()
        .HasIndex(b => new {b.EmployeeId, b.BenefitId}).IsUnique();
    }
}
  
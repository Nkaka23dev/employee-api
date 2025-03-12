using Microsoft.EntityFrameworkCore;

namespace TheEmployeeAPI;

public class AppBbContext: DbContext
{
 public DbSet<Employee> Employees {get; set;}
 public AppBbContext(DbContextOptions<AppBbContext> options): base(options){

 }
}

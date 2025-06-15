using Microsoft.EntityFrameworkCore;
using PayrollService.Models;

namespace PayrollService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt): base(opt)
    {
        
    }
    public DbSet<Payroll> Payrolls { get; set; }
}

using PayrollService.Models;

namespace PayrollService.Data;

public class PayrollRepo : IPayrollRepo
{
    private readonly AppDbContext _context;

    public PayrollRepo(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }
    public void CreatePayroll(Payroll payroll)
    {
        ArgumentNullException.ThrowIfNull(payroll);
        _context.Payrolls.Add(payroll);
    }
        public IEnumerable<Payroll> GetPayrolls()
    {
            return _context.Payrolls.ToList();
    }
    public Payroll? GetPayrollById(int id)
    {
        return _context.Payrolls.FirstOrDefault(p => p.Id == id);
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}

using System;
using PayrollService.Models;

namespace PayrollService.Data;

public interface IPayrollRepo
{
    bool SaveChanges();
    IEnumerable<Payroll> GetPayrolls();
    Payroll? GetPayrollById(int id);
    void CreatePayroll(Payroll payroll);
}

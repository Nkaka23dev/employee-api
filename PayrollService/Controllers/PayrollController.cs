using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PayrollService.Data;
using PayrollService.DTOs;
using PayrollService.Models;

namespace PayrollService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PayrollController(IPayrollRepo repository, IMapper mapper) : ControllerBase
{
    private readonly IPayrollRepo _repository = repository;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public ActionResult<IEnumerable<Payroll>> GetPayroll()
    {
        Console.WriteLine("---> Getting payroll....");
        var payrollItems = _repository.GetPayrolls();
        return Ok(_mapper.Map<IEnumerable<PayrollReadDTOs>>(payrollItems));
    }
    [HttpGet("{id}")]
    public ActionResult<Payroll> GetPayrollById(int id)
    {
        Console.WriteLine("---> Getting payroll by ID....");
        var payrollItem = _repository.GetPayrollById(id);
        if (payrollItem != null)
        {
            return Ok(_mapper.Map<PayrollReadDTOs>(payrollItem));
        }
        return NotFound();
    }
}
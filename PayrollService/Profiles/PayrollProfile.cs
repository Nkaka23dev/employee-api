using AutoMapper;
using PayrollService.DTOs;
using PayrollService.Models;

namespace PayrollService.Profiles;

public class PayrollProfile : Profile
{
    public PayrollProfile()
    {
        //source - target
        CreateMap<Payroll, PayrollReadDTOs>();
        CreateMap<PayrollCreateDTO, Payroll>();
    }
}

using AutoMapper;
using BenefitSoapService.Contacts;
using Core.Domain.DTOs.Benefits;
using TheEmployeeAPI.Domain.Entities;

namespace BenefitSoapService.Application.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
         {
        CreateMap<Benefit, BenefitContract>().ReverseMap();
        CreateMap<Benefit, GetBenefitResponse>();
        CreateMap<UpdateBenefit, Benefit>();
    }
    }
}

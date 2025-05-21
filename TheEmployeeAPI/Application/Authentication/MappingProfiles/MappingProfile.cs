using AutoMapper;
using TheEmployeeAPI.Domain.DTOs.Authentication;
using TheEmployeeAPI.Domain.DTOs.Users;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Application.Authentication.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserResponse>()
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
            CreateMap<ApplicationUser, CurrentUserResponse>();
            CreateMap<RegisterRequest, ApplicationUser>();

        }
    }
}

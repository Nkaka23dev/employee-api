using AutoMapper;
using TheEmployeeAPI.Contracts.User;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Application.Authentication.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<ApplicationUser, CurrentUserResponse>();
            CreateMap<RegisterRequest, ApplicationUser>();

        }
    }
}

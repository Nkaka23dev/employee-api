using AutoMapper;
using TheEmployeeAPI.Contracts.User;
using TheEmployeeAPI.Domain;


namespace TheEmployeeAPI.Mappings
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

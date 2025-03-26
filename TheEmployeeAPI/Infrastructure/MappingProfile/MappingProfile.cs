using AutoMapper;
using TheEmployeeAPI.Domain.Contracts.Auth;
using TheEmployeeAPI.Entities.Auth;

namespace TheEmployeeAPI.Infrastructure.MappingProfile;

public class MappingProfile: Profile 
{
 public MappingProfile()
 {
    CreateMap<ApplicationUser, UserResponse>();
    CreateMap<ApplicationUser, CurrentUserResponse>();
    CreateMap<UserRegisterRequest, ApplicationUser>();
 }
}

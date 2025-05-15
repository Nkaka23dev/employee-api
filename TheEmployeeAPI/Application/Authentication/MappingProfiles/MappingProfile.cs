using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using TheEmployeeAPI.Domain.DTOs.Users;
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

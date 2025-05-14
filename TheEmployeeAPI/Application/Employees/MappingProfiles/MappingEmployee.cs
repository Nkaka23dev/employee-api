using AutoMapper;
using TheEmployeeAPI.Application.Employees.DTOs;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Application.Employees.MappingProfiles
{
    public class MappingEmployee : Profile
    {
        public MappingEmployee()
        {
            CreateMap<CreateEmployeeRequest, Employee>();
            CreateMap<UpdateEmployeeRequest, Employee>();
            CreateMap<Employee, GetEmployeeResponse>();

        }
    }
}

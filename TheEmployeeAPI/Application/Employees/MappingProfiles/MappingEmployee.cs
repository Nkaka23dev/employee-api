using AutoMapper;
using TheEmployeeAPI.Contracts.Employee;
using TheEmployeeAPI.Entities.Employee;

namespace TheEmployeeAPI.Mappings
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

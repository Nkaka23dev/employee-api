using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.DTOs.Employees;
using TheEmployeeAPI.Domain.Entities;
using TheEmployeeAPI.Persistance.Repositories;

namespace TheEmployeeAPI.Application.Employees.Services
{
    public class EmployeeService(
        ILogger<EmployeeService> logger,
        IMapper mapper,
        IEmployeeRepository employeeRepository) : IEmployeeService
    {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        public async Task<IEnumerable<GetEmployeeResponse>> GetAllEmployeesAsync(GetAllEmployeesRequest request)
        {
            var page = request?.Page ?? 1;
            var numberOfRecord = request?.RequestPerPage ?? 100;

            IQueryable<Employee> query = _employeeRepository.GetQuery(page, numberOfRecord);

            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.FirstNameContains))
                {
                    query = query.Where(e => e.FirstName.Contains(request.FirstNameContains));
                }
                if (!string.IsNullOrWhiteSpace(request.LastNameContains))
                {
                    query = query.Where(e => e.LastName.Contains(request.LastNameContains));
                }
            }
            var employees = await query.ToArrayAsync();
            return _mapper.Map<List<GetEmployeeResponse>>(employees);

        }
        public async Task<GetEmployeeResponse> GetEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id)
             ?? throw new KeyNotFoundException($"Employee with {id} not found!");

            var employeeResponse = _mapper.Map<GetEmployeeResponse>(employee);
            return employeeResponse;
        }

        public async Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            var newEmployee = _mapper.Map<Employee>(request);
            await _employeeRepository.AddAsync(newEmployee);
            return newEmployee;
        }

        public async Task<GetEmployeeResponse>
         UpdateEmployeeAsync(int id, UpdateEmployeeRequest request)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                _logger.LogWarning("Employee with ID {employeeId} NOT FOUND!", id);
                throw new KeyNotFoundException($"Emplouyee with {id} not found!");

            }
            _mapper.Map(request, existingEmployee);

            await _employeeRepository.UpdateAsync(existingEmployee);

            var employeeResponse = _mapper.Map<GetEmployeeResponse>(existingEmployee);
            return employeeResponse;
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            await _employeeRepository.DeleteAsync(id);

        }
        public async Task<IEnumerable<GetEmployeeResponseEmployeeBenefits>>
         GetBenefitsForEmployeeAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetBenefits(employeeId);
            var benefits = employee.Benefits.Select(b => new GetEmployeeResponseEmployeeBenefits
            {
                Id = b.Id,
                Name = b.Benefit.Name,
                Description = b.Benefit.Description,
                Cost = b.CostToEmployee ?? b.Benefit.BaseCost
            });
            return benefits;
        }
    }
}

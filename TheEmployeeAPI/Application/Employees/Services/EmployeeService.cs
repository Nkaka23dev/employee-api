using AutoMapper;
using Core.Domain.DTOs;
using Core.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.DTOs.Employees;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Application.Employees.Services
{
    public class EmployeeService(
        ILogger<EmployeeService> logger,
        IMapper mapper,
        IEmployeeRepository employeeRepository,
        IBenefitRepository benefitRepository) : IEmployeeService
    {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IBenefitRepository _benefitRepository = benefitRepository;
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

        public async Task<GetEmployeeResponse> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            var newEmployee = _mapper.Map<Employee>(request);
            await AssignBenefitToEmployee(newEmployee, request);
            await _employeeRepository.AddAsync(newEmployee);
            var employeeResponse = _mapper.Map<GetEmployeeResponse>(newEmployee);
            return employeeResponse;
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
            await AssignBenefitToEmployee(existingEmployee, request);
         
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

        public async Task AssignBenefitToEmployee(Employee employee, IEmployeeBenefitRequest ids)
        {
            if (ids.BenefitsIds != null && ids.BenefitsIds.Count != 0)
            {
                var benefits = await _benefitRepository.GetByBenefitIdsAsync(ids.BenefitsIds);
                var foundIds = benefits.Select(b => b.Id).ToHashSet();
                var notFoundIds = ids.BenefitsIds.Where(id => !foundIds.Contains(id)).ToList();

                if (notFoundIds.Count != 0)
                {
                    throw new Exception($"Benefit not found: {string.Join(", ", notFoundIds)}");

                }
                foreach (var benefit in benefits)
                {
                    employee.Benefits.Add(new EmployeeBenefit
                    {
                        BenefitId = benefit.Id,
                        Benefit = benefit,
                        CostToEmployee = benefit.BaseCost
                    });
                }

            }

        }
    }
}

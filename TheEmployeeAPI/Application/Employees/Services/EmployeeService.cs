using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Contracts.Employee;
using TheEmployeeAPI.Entities.Employee;
using TheEmployeeAPI.Infrastructure.Context;
using TheEmployeeAPI.Services.Employees;

namespace TheEmployeeAPI.Application.Employees
{
    public class EmployeeService(
        AppDbContext dbContext,
        ILogger<EmployeeService> logger,
        IMapper mapper) : IEmployeeService
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<GetEmployeeResponse>> GetAllEmployeesAsync(GetAllEmployeesRequest request)
        {
            var page = request?.Page ?? 1;
            var numberOfRecord = request?.RequestPerPage ?? 100;

            IQueryable<Employee> query = _dbContext.Employees
            .Include(e => e.Benefits)
            .Skip((page - 1) * numberOfRecord)
            .Take(numberOfRecord);

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
            return employees.Select(EmployeeToGetEmployeeResponse);
        }
        public async Task<GetEmployeeResponse> GetEmployeeAsync(int id)
        {
            var employee = await _dbContext.Employees.SingleOrDefaultAsync(e => e.Id == id)
             ?? throw new KeyNotFoundException($"Employee with {id} not found!");

            var employeeResponse = EmployeeToGetEmployeeResponse(employee);
            return employeeResponse;
        }

        public async Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            var newEmployee = _mapper.Map<Employee>(request);
            _dbContext.Employees.Add(newEmployee);
            await _dbContext.SaveChangesAsync();
            return newEmployee;
        }

        public async Task<GetEmployeeResponse> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request)
        {
            var existingEmployee = await _dbContext.Employees.SingleOrDefaultAsync(e => e.Id == id);
            if (existingEmployee == null)
            {
                _logger.LogWarning("Employee with ID {employeeId} NOT FOUND!", id);
                throw new KeyNotFoundException($"Emplouyee with {id} not found!");

            }
            _mapper.Map(request, existingEmployee);

            _dbContext.Entry(existingEmployee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            var employeeResponse = _mapper.Map<GetEmployeeResponse>(existingEmployee);
            return employeeResponse;
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id)
            ?? throw new Exception($"Employee with {id} not found!");

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<GetEmployeeResponseEmployeeBenefits>> GetBenefitsForEmployeeAsync(int employeeId)
        {
            var employee = await _dbContext.Employees
              .Include(e => e.Benefits)
              .ThenInclude(e => e.Benefit)
              .SingleOrDefaultAsync(e => e.Id == employeeId)
               ?? throw new KeyNotFoundException($"Employee with {employeeId} not found!");
            var benefits = employee.Benefits.Select(b => new GetEmployeeResponseEmployeeBenefits
            {
                Id = b.Id,
                Name = b.Benefit.Name,
                Description = b.Benefit.Description,
                Cost = b.CostToEmployee ?? b.Benefit.BaseCost
            });
            return benefits;
        }
        private static GetEmployeeResponse EmployeeToGetEmployeeResponse(Employee employee)
        {
            return new GetEmployeeResponse
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address1 = employee.Address1,
                Address2 = employee.Address2,
                City = employee.City,
                Email = employee.Email,
                ZipCode = employee.ZipCode,
                PhoneNumber = employee.PhoneNumber,
                State = employee.State,
                CreatedBy = employee.CreatedBy,
                LastModifiedBy = employee.LastModifiedBy,
                CreatedOn = employee.CreatedOn,
                LastModifiedOn = employee.LastModifiedOn
            };
        }
    }
}

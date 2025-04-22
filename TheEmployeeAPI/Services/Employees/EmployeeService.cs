using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Contracts.Employee;
using TheEmployeeAPI.Entities.Employee;
using TheEmployeeAPI.Infrastructure.Context;

namespace TheEmployeeAPI.Services.Employees;

public class EmployeeService(AppDbContext dbContext) : IEmployeeService
{ 
    private readonly AppDbContext _dbContext = dbContext;
    public async Task<IEnumerable<GetEmployeeResponse>> GetAllEmployeesAsync(GetAllEmployeesRequest request)
    {
        var page = request?.Page ?? 1;
        var numberOfRecord = request?.RequestPerPage ?? 100; 

        IQueryable<Employee> query = _dbContext.Employees
        .Include(e => e.Benefits)
        .Skip((page - 1) * numberOfRecord)
        .Take(numberOfRecord);

        if(request != null){
            if(!string.IsNullOrWhiteSpace(request.FirstNameContains)){
                query = query.Where(e => e.FirstName.Contains(request.FirstNameContains));
            }
            if(!string.IsNullOrWhiteSpace(request.LastNameContains)){
                query = query.Where(e => e.LastName.Contains(request.LastNameContains));
            }
        }
        var employees = await query.ToArrayAsync();
        return employees.Select(EmployeeToGetEmployeeResponse);
    }

    private static GetEmployeeResponse EmployeeToGetEmployeeResponse(Employee employee){
    return new GetEmployeeResponse {
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

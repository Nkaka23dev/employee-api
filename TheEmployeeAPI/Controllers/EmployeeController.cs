using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.dtos;

namespace TheEmployeeAPI.employees;

public class EmployeeController: BaseController
{
 private readonly ILogger<EmployeeController> _logger;
 private readonly AppBbContext _dbContext ;
 public EmployeeController(ILogger<EmployeeController> logger, AppBbContext dbContext)
 {
    _logger = logger;
    _dbContext = dbContext;
 }
  
 /// <summary>
 /// Get All of The Employees In The System
 /// </summary> 
 /// <returns>Returns the employees in JSON array.</returns>
 [HttpGet("all")]
 [ProducesResponseType(typeof(IEnumerable<GetEmployeeResponse>), StatusCodes.Status200OK)]
 [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
 public async Task<IActionResult> GetAllEmployees([FromQuery] GetAllEmployeesRequest request){
 
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
   return Ok(employees.Select(EmployeeToGetEmployeeResponse));
 } 
 /// <summary>
 /// Get an Employee by id.
 /// </summary> 
 /// <param name="id">ID of an employee you want to get</param>
 /// <returns>Return employee object</returns>
 [HttpGet("{id}")]
 [ProducesResponseType(typeof(GetEmployeeResponse), StatusCodes.Status200OK)]
 [ProducesResponseType(StatusCodes.Status404NotFound)]
 [ProducesResponseType(StatusCodes.Status500InternalServerError)]
 public async Task<IActionResult> GetEmployeeById(int id){ 
   var employee =  await _dbContext.Employees.SingleOrDefaultAsync(e => e.Id == id);
   if(employee == null){
     return NotFound();
   }
   var employeeResponse = EmployeeToGetEmployeeResponse(employee);
   return Ok(employee);
 }
 /// <summary>
 ///  Create a new Employee
 /// </summary>
 /// <param name="employeeRequest">Object containing required field to create employee</param>
 /// <returns>Return 201 created</returns>
 [HttpPost]
 [ProducesResponseType(typeof(GetEmployeeResponse),StatusCodes.Status201Created)]
 [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
 [ProducesResponseType(StatusCodes.Status500InternalServerError)]
 public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest employeeRequest){
   await Task.CompletedTask;
    var newEmployee = new Employee {
        FirstName = employeeRequest.FirstName!,
        LastName = employeeRequest.LastName!,
        SocialSecurityNumber = employeeRequest.SocialSecurityNumber,
        Address1 = employeeRequest.Address1,
        Address2 = employeeRequest.Address2,
        City = employeeRequest.City,
        State = employeeRequest.State,
        ZipCode = employeeRequest.ZipCode,
        PhoneNumber = employeeRequest.PhoneNumber,
        Email = employeeRequest.Email, 
    };
   _dbContext.Employees.Add(newEmployee);
   await _dbContext.SaveChangesAsync();
   return CreatedAtAction(nameof(GetEmployeeById), new {id = newEmployee.Id}, newEmployee);
 }
 /// <summary>
 /// Update an Employee
 /// </summary> 
 /// <param name="id">The Id of an Employee to Update.</param>
 /// <param name="employee">The Employee data to update</param>
 /// <returns>Return Updated Employee</returns>
 [HttpPut("{id}")]
 [ProducesResponseType(typeof(GetEmployeeResponse),StatusCodes.Status200OK)]
 [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
 [ProducesResponseType(StatusCodes.Status404NotFound)]
 [ProducesResponseType(StatusCodes.Status500InternalServerError)]
 public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeRequest employee){

   var existingEmployee = await _dbContext.Employees.FindAsync(id);
  //  var existingEmployee = await _dbContext.Employees
  //  .AsTracking().SingleOrDefaultAsync(e => e.Id == id);
   if(existingEmployee == null){
      _logger.LogWarning("Employee with ID {employeeId} NOT FOUND!", id);
      return NotFound();
    }
    existingEmployee.Address1 = employee.Address1;
    existingEmployee.Address2 = employee.Address2;
    existingEmployee.City = employee.City;
    existingEmployee.State = employee.State;
    existingEmployee.ZipCode = employee.ZipCode;
    existingEmployee.PhoneNumber = employee.PhoneNumber;
    existingEmployee.Email = employee.Email;
    
    try
    {
      _dbContext.Entry(existingEmployee).State = EntityState.Modified;
      await _dbContext.SaveChangesAsync();
       _logger.LogInformation("Employee with ID: {employeeID} successfuly updated", id);
      return Ok(existingEmployee); 
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error occured while updated employee with ID {employeeID}", id);
      return StatusCode(500, "Erro occured while updating employee");
 
    }
 }
 /// <summary>
 /// Delete Employee
 /// </summary>
 /// <param name="id">Id of an employee to be deleted</param>
 /// <returns>No content for deleted employee</returns>
 [HttpDelete("{id}")]
 [ProducesResponseType(StatusCodes.Status204NoContent)]
 [ProducesResponseType(StatusCodes.Status404NotFound)]
 [ProducesResponseType(StatusCodes.Status500InternalServerError)]
 public async Task<IActionResult> DeleteEmployee([FromRoute] int id){
  var employee = await _dbContext.Employees.FindAsync(id);

  if(employee == null){

    return NotFound();
  }
  _dbContext.Employees.Remove(employee);
  await _dbContext.SaveChangesAsync();

  return NoContent();
 }
     /// <summary>
    /// Gets the benefits for an employee.
    /// </summary>
    /// <param name="employeeId">The ID to get the benefits for.</param>
    /// <returns>The benefits for that employee.</returns>
    [HttpGet("{employeeId}/benefits")]
    [ProducesResponseType(typeof(IEnumerable<GetEmployeeResponseEmployeeBenefits>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBenefitsForEmployee(int employeeId)
    {
        var employee = await _dbContext.Employees
            .Include(e => e.Benefits)
            .ThenInclude(e => e.Benefit)
            .SingleOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
        {
            return NotFound();
        }

        var benefits = employee.Benefits.Select(b => new GetEmployeeResponseEmployeeBenefits
        {
            Id = b.Id,
            Name = b.Benefit.Name,
            Description = b.Benefit.Description,
            Cost = b.CostToEmployee ?? b.Benefit.BaseCost
        });

        return Ok(benefits);
    }

 
private GetEmployeeResponse EmployeeToGetEmployeeResponse(Employee employee){
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
 
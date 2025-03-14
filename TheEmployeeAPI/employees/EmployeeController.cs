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
//  /// <summary>
//  ///  Create a new Employee
//  /// </summary>
//  /// <param name="employeeRequest">Object containing required field to create employee</param>
//  /// <returns>Return 201 created</returns>
//  [HttpPost]
//  [ProducesResponseType(typeof(GetEmployeeResponse),StatusCodes.Status201Created)]
//  [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
//  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//  public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest employeeRequest){
//    await Task.CompletedTask;
//     var newEmployee = new Employee {
//         FirstName = employeeRequest.FirstName!,
//         LastName = employeeRequest.LastName!,
//         SocialSecurityNumber = employeeRequest.SocialSecurityNumber,
//         Address1 = employeeRequest.Address1,
//         Address2 = employeeRequest.Address2,
//         City = employeeRequest.City,
//         State = employeeRequest.State,
//         ZipCode = employeeRequest.ZipCode,
//         PhoneNumber = employeeRequest.PhoneNumber,
//         Email = employeeRequest.Email, 
//     };
//   _repository.Create(newEmployee);
//   return CreatedAtAction(nameof(GetEmployeeById),
//    new {id = newEmployee.Id}, newEmployee);
//  }
//  /// <summary>
//  /// Update an Employee
//  /// </summary>
//  /// <param name="id">The Id of an Employee to Update.</param>
//  /// <param name="employee">The Employee data to update</param>
//  /// <returns>Return Updated Employee</returns>
//  [HttpPut("{id}")]
//  [ProducesResponseType(typeof(GetEmployeeResponse),StatusCodes.Status200OK)]
//  [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
//  [ProducesResponseType(StatusCodes.Status404NotFound)]
//  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//  public IActionResult UpdateEmployee(int id, [FromBody]
//   UpdateEmployeeRequest employee){
//    _logger.LogInformation("Updatating employee with ID: {employeeID}", id);
//    var existingEmployee = _repository.GetById(id);
//    if(existingEmployee == null){
//       _logger.LogWarning("Employee with ID {employeeId} NOT FOUND!", id);
//       return NotFound();
//     }
//     existingEmployee.Address1 = employee.Address1;
//     existingEmployee.Address2 = employee.Address2;
//     existingEmployee.City = employee.City;
//     existingEmployee.State = employee.State;
//     existingEmployee.ZipCode = employee.ZipCode;
//     existingEmployee.PhoneNumber = employee.PhoneNumber;
//     existingEmployee.Email = employee.Email;
//     _repository.Update(existingEmployee);
//     return Ok(existingEmployee);
//  } 

//  /// <summary> 
//  /// Get benefits of an Employee
//  /// </summary>
//  /// <param name="employeeId"></param>
//  /// <returns>The Benefits for that employee</returns>
//  [HttpGet("{employeeId}/benefits")]
//  [ProducesResponseType(typeof(IEnumerable<GetEmployeeResponseEmployeeBenefits>),
//   StatusCodes.Status200OK)]
//  [ProducesResponseType(StatusCodes.Status404NotFound)]
//  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//  public IActionResult GetBenefitsForEmployee(int employeeId){ 
//    var employee = _repository.GetById(employeeId);
//    if(employee == null){
//      return NotFound(); 
//    }
//    return Ok(employee.Benefits.Select(BenefitsToBenefitsResponse));
//  } 

private static GetEmployeeResponseEmployeeBenefits 
BenefitsToBenefitsResponse(EmployeeBenefits employeeBenefits){
 return new GetEmployeeResponseEmployeeBenefits {
  Id = employeeBenefits.Id,
  EmployeeId = employeeBenefits.EmployeeId,
  BenefitsType = employeeBenefits.BenefitsType,
  Cost = employeeBenefits.Cost
 };
}

private GetEmployeeResponse 
EmployeeToGetEmployeeResponse(Employee employee){
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
       Benefits = employee.Benefits.Select(benefit => new GetEmployeeResponseEmployeeBenefits{
          Id = benefit.Id,
          EmployeeId = benefit.EmployeeId,
          BenefitsType = benefit.BenefitsType,
          Cost = benefit.Cost
       }).ToList()
   };
}
}

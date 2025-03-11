using Microsoft.AspNetCore.Mvc;
using TheEmployeeAPI.abstraction;

namespace TheEmployeeAPI.employees;

public class EmployeeController: BaseController
{
 private readonly IRepository<Employee> _repository;
  private readonly ILogger<EmployeeController> _logger;

 public EmployeeController(IRepository<Employee> repository, 
 ILogger<EmployeeController> logger)
 {
    _repository = repository;
    this._logger = logger;
 }
 [HttpGet("all")]
 public IActionResult GetAllEmployees(){
    var employeeResponse = _repository.GetAll()
    .Select(employee => new GetEmployeeRequest {
       FirstName = employee.FirstName, 
       LastName = employee.LastName,
       Address1 = employee.Address1,
       Address2 = employee.Address2,
       City = employee.City,
       Email = employee.Email,
       ZipCode = employee.ZipCode,
       PhoneNumber = employee.PhoneNumber,     
       State = employee.State  
   });
    return Ok(employeeResponse);
 }

 [HttpGet("{id}")]
 public IActionResult GetEmployeeById(int id){
   var employee = _repository.GetById(id);
   if(employee == null){
     return NotFound();
   }
   var employeeResponse = new GetEmployeeRequest {
       FirstName = employee.FirstName,
       LastName = employee.LastName,
       Address1 = employee.Address1,
       Address2 = employee.Address2,
       City = employee.City,
       Email = employee.Email,
       ZipCode = employee.ZipCode,
       PhoneNumber = employee.PhoneNumber,     
       State = employee.State  
   };
   return Ok(employeeResponse);
 }
 [HttpPost]
 public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest employeeRequest){
    var validationResults = await ValidateAsync(employeeRequest);
    if (!validationResults.IsValid)
    {
        return ValidationProblem(validationResults.ToModelStateDictionary());
    }
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
  _repository.Create(newEmployee);
  return CreatedAtAction(nameof(GetEmployeeById),
   new {id = newEmployee.Id}, newEmployee);
 }

 [HttpPut("{id}")]
 public IActionResult UpdateEmployee(int id, [FromBody]
  UpdateEmployeeRequest employee){
   _logger.LogInformation("Updatating employee with ID: {employeeID}", id);
   var existingEmployee = _repository.GetById(id);
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
    _repository.Update(existingEmployee);
    return Ok(existingEmployee);
 }
}

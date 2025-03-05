using Microsoft.AspNetCore.Mvc;
using TheEmployeeAPI;
using TheEmployeeAPI.abstraction;
using TheEmployeeAPI.employees;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IRepository<Employee>, EmployeeRepository>();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Employees API documentation";
    config.Title = "Employees API v1";
    config.Version = "v1";
});
var app = builder.Build();
  
var employeeRoute = app.MapGroup("employees");

if (app.Environment.IsDevelopment())
{   
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "Employees API";
        config.Path = "/api";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.UseHttpsRedirection();

employeeRoute.MapGet(string.Empty, ([FromServices] IRepository<Employee> repository) => {
   var employees = repository.GetAll();
   return Results.Ok(employees.Select((employee) => new GetEmployeeResponse
   {
       FirstName = employee.FirstName,
       LastName = employee.LastName,
       Address1 = employee.Address1,
       Address2 = employee.Address2,
       City = employee.City,
       Email = employee.Email,
       ZipCode = employee.ZipCode,
       PhoneNumber = employee.PhoneNumber,     
       State = employee.State  
   })); 
});

employeeRoute.MapGet("{id:int}", ( [FromServices] IRepository<Employee> repository, 
[FromRoute] int id) => {
    var employee = repository.GetById(id);
    if(employee == null){ 
        return Results.NotFound();
    }
    return Results.Ok(new GetEmployeeResponse
   {
       FirstName = employee.FirstName,
       LastName = employee.LastName,
       Address1 = employee.Address1,
       Address2 = employee.Address2,
       City = employee.City,
       Email = employee.Email,
       ZipCode = employee.ZipCode,
       PhoneNumber = employee.PhoneNumber,     
       State = employee.State  
   });;
});

employeeRoute.MapPut("{id}", ([FromServices] IRepository<Employee> repository, [FromBody] 
UpdateEmployeeRequest employee, int id) => {
  var existingEmployee = repository.GetById(id);
  if(existingEmployee == null){
    return Results.NotFound();
  }
    existingEmployee.Address1 = employee.Address1;
    existingEmployee.Address2 = employee.Address2;
    existingEmployee.City = employee.City;
    existingEmployee.State = employee.State;
    existingEmployee.ZipCode = employee.ZipCode;
    existingEmployee.PhoneNumber = employee.PhoneNumber;
    existingEmployee.Email = employee.Email;
  repository.Update(existingEmployee);
  return Results.Ok(existingEmployee.FirstName);
});

employeeRoute.MapPost(string.Empty, ([FromServices] IRepository<Employee> repository,
[FromBody] CreateEmployeeRequest employee, HttpContext context) => {
    var newEmployee = new Employee {
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        SocialSecurityNumber = employee.SocialSecurityNumber,
        Address1 = employee.Address1,
        Address2 = employee.Address2,
        City = employee.City,
        State = employee.State,
        ZipCode = employee.ZipCode,
        PhoneNumber = employee.PhoneNumber,
        Email = employee.Email,
    };
    repository.Create(newEmployee);
    return Results.Created($"employees/{newEmployee.Id}", employee);
});

app.Run();

public partial class Program {}
using Microsoft.AspNetCore.Mvc;
using TheEmployeeAPI.employees;

var builder = WebApplication.CreateBuilder(args);
var employees = new List<Employee> {
    new Employee {Id = 1, FirstName= "Eric", LastName="Nkaka", SocialSecurityNumber="6575-574-6544"},
    new Employee {Id = 2, FirstName= "Joe", LastName="Doe", SocialSecurityNumber="1121-4334-1111"}
};
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
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

employeeRoute.MapGet(string.Empty, () => {
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

employeeRoute.MapGet("{id:int}", ([FromRoute] int id) => {
    var employee = employees.SingleOrDefault(e => e.Id == id);
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

employeeRoute.MapPut("{id}",([FromBody] UpdateEmployeeRequest employee, int id) => {
  var existingEmployee = employees.SingleOrDefault(e => e.Id == id);
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

  return Results.Ok($"{existingEmployee.FirstName} updated successfully!");
});

employeeRoute.MapPost(string.Empty, ([FromBody] CreateEmployeeRequest employee, HttpContext context) => {
    var newEmployee = new Employee {
        Id = employees.Max(e => e.Id) + 1,
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
    employees.Add(newEmployee);
    return Results.Created($"employees/{newEmployee.Id}", employee);
});

app.Run();

public partial class Program {}
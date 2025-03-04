using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var employees = new List<Employee> {
    new Employee {Id = 1, FirstName= "Eric", LastName="Nkaka"},
    new Employee {Id = 2, FirstName= "Joe", LastName="Doe"}
};
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "gApi documentation";
    config.Title = "gAPI v1";
    config.Version = "v1";
});
var app = builder.Build();

var employeeRoute = app.MapGroup("employees");

if (app.Environment.IsDevelopment())
{   
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "gAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.UseHttpsRedirection();

employeeRoute.MapGet(string.Empty, () => {
   return Results.Ok(employees);
});

employeeRoute.MapGet("{id:int}", ([FromRoute] int id) => {
    var employee = employees.SingleOrDefault(e => e.Id == id);
    if(employee == null){
        return Results.NotFound();
    }
    return Results.Ok(employee);
});

employeeRoute.MapPost(string.Empty, ([FromBody]Employee employee, HttpContext context) => {
    employee.Id = employees.Max(e => e.Id) + 1; 
    employees.Add(employee);
    return Results.Created($"employees/{employee.Id}", employee);
});

app.Run();


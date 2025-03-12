using TheEmployeeAPI;
using TheEmployeeAPI.abstraction;
using FluentValidation;

var employees = new List<Employee>
{
    new Employee { Id = 1, FirstName = "John", LastName = "Doe" },
    new Employee { Id = 2, FirstName = "Jane", LastName = "Doe" }
};

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();
builder.Services.AddSingleton<IRepository<Employee>, EmployeeRepository>();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddControllers(options => {
    options.Filters.Add<FluentValidationFilter>();
});
builder.Services.AddHttpContextAccessor();

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
        config.Path =  "/api";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program {}
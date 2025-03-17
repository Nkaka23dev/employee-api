using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TheEmployeeAPI.employees;
 
namespace TheEmployeeAPI.Tests;

public class BasicTests: IClassFixture<CustomWebApplicationFactory>{
    private readonly int _employeeId = 1;

    private readonly CustomWebApplicationFactory _factory; 
    public BasicTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }
    [Fact]
    public async Task GetAllEmployees_ReturnOkResults(){

        var client = _factory.CreateClient();
        var response  = await client.GetAsync("/employee/all");
        
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get employees: {content}");
        }

        var employees = await response.Content.ReadFromJsonAsync<IEnumerable<GetEmployeeResponse>>();
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
    }

    [Fact]
    public async Task GetAllEmployees_WithFilter_ReturnsOneResult()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/employee/all?FirstNameContains=Jane");

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Failed to get employees: {content}");
        }

        var employees = await response.Content.ReadFromJsonAsync<IEnumerable<GetEmployeeResponse>>();
        Assert.NotNull(employees);
        Assert.Single(employees);
    }
    
    [Fact]
    public async Task GetEmployeeById_ReturnOkResult(){

        var client = _factory.CreateClient();
        var response  = await client.GetAsync("/employee/1");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateEmployee_ReturnsCreatedResult(){
     var client  = _factory.CreateClient();
     var response = await client.PostAsJsonAsync("/employee",
         new Employee { FirstName = "John", LastName ="Doe",  SocialSecurityNumber="6575-574-6544"});
     response.EnsureSuccessStatusCode();
    }


    [Fact]
    public async Task CreateEmployees_ReturnsBadRequestResult()
    {
        // Arranging
        var client = _factory.CreateClient();
        // Reason: Empty object to trigger validation errors
        var invalidEmployee = new CreateEmployeeRequest(); 
        // Act
        var response = await client.PostAsJsonAsync("/employee", invalidEmployee);

        // Asserting
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Contains("FirstName", problemDetails.Errors.Keys);
        Assert.Contains("LastName", problemDetails.Errors.Keys);
        Assert.Contains("'First Name' must not be empty.", problemDetails.Errors["FirstName"]);
        Assert.Contains("'Last Name' must not be empty.", problemDetails.Errors["LastName"]);
    }
 
    [Fact]
    public async Task UpdateEmployee_ReturnsOkResult()
    {
        var client = _factory.CreateClient();
        var response = await client.PutAsJsonAsync("/employee/1", new Employee { 
            FirstName = "John",
            LastName = "Doe", 
            Address1 = "123 Main Smoot",
            });
        response.EnsureSuccessStatusCode();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppBbContext>();
        var employee = await db.Employees.FindAsync(1);
        Assert.Equal("123 Main Smoot", employee?.Address1);
    } 
 
    [Fact] 
    public async Task UpdateEmployee_ReturnBadRequestWhenAddress1UpdatedToEmpty()
    {   
        var client  = _factory.CreateClient();
        var invalidEmployee = new UpdateEmployeeRequest(); 

        var response = await client.PutAsJsonAsync($"/employee/{_employeeId}", invalidEmployee);
        Console.Write($"{HttpStatusCode.BadRequest}, {response.StatusCode}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problemDetails); 
        Assert.Contains("Address1", problemDetails.Errors.Keys);       

    }
    [Fact]
    public async Task DeleteEmployee_ReturnNoContentResults(){
        var client = _factory.CreateClient();
        var newEmployee = new Employee {FirstName = "Meow", LastName="Garitea"};
        using (var scope = _factory.Services.CreateScope()){
          var db = scope.ServiceProvider.GetRequiredService<AppBbContext>();
          db.Employees.Add(newEmployee);
          await db.SaveChangesAsync();
        }
        var response = await client.DeleteAsync($"/employee/{newEmployee.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteEmployee_ReturnNotFoundResult(){
        var client = _factory.CreateClient();
        var response = await client.DeleteAsync("/employee/99999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
   
//     [Fact]
//     public async Task GetBenefitsForEmployee_ReturnsOkResult()
//     {
//         // Act
//         var client = _factory.CreateClient();
//         var response = await client.GetAsync($"/employee/{_employeeId}/benefits");

//         // Assert
//         response.EnsureSuccessStatusCode();
        
//         var benefits = await response.Content.ReadFromJsonAsync<IEnumerable<GetEmployeeResponseEmployeeBenefits>>();
//         Assert.Equal(2, benefits!.Count());
//     }
}
/*
    Assert.True(response.IsSuccessStatusCode)
    can also work but prefered response.EnsureSuccessStatusCode()
*/
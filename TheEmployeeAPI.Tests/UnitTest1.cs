using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using TheEmployeeAPI.employees;

namespace TheEmployeeAPI.Tests;

public class BasicTests: IClassFixture<WebApplicationFactory<Program>>{
    private readonly WebApplicationFactory<Program>_factory;
    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    [Fact]
    public async Task GetAllEmployees_ReturnOkResults(){

        var client = _factory.CreateClient();
        var response  = await client.GetAsync("/employee/all");
        response.EnsureSuccessStatusCode();
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
        // Arrange
        var client = _factory.CreateClient();
        var invalidEmployee = new CreateEmployeeRequest(); // Empty object to trigger validation errors

        // Act
        var response = await client.PostAsJsonAsync("/employee", invalidEmployee);

        // Assert
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
        var response = await client.PutAsJsonAsync("/employee/1", new Employee { FirstName = "John", LastName = "Doe" });

        response.EnsureSuccessStatusCode();
    }
}
/*
    Assert.True(response.IsSuccessStatusCode)
    can also work but prefered response.EnsureSuccessStatusCode()
*/
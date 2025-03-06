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
        var response  = await client.GetAsync("/employees");
        response.EnsureSuccessStatusCode();
    }
    [Fact]
    public async Task GetEmployeeById_ReturnOkResult(){

        var client = _factory.CreateClient();
        var response  = await client.GetAsync("/employees/1");
        response.EnsureSuccessStatusCode();
    }
    [Fact]
    public async Task CreateEmployee_ReturnsCreatedResult(){
     var client  = _factory.CreateClient();
     var response = await client.PostAsJsonAsync("/employees",
         new Employee { FirstName = "John", LastName ="Doe",  SocialSecurityNumber="6575-574-6544"});
     response.EnsureSuccessStatusCode();
    }

    [Fact] 
    public async Task CreateEmployee_ReturnsBadRequestResult(){

     //Arrange
     var client = _factory.CreateClient();
     var invalidEmployee = new CreateEmployeeRequest(); //Empty object;

     //Act
     var response = await client.PostAsJsonAsync("/employees", invalidEmployee);

     //Assert
     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
     var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(); 
     Assert.NotNull(problemDetails);
     Assert.Contains("FirstName", problemDetails.Errors.Keys);
     Assert.Contains("LastName", problemDetails.Errors.Keys);
     Assert.Contains("The FirstName field is required.", problemDetails.Errors["FirstName"]);
     Assert.Contains("The LastName field is required.", problemDetails.Errors["LastName"]);
    }

    // [Fact]
    //  public async Task UpdateEmployee_ReturnOkResults() {
    //     var client = _factory.CreateClient();
    //     var response = await client.PutAsJsonAsync("/employees/2", new Employee { FirstName = "John", LastName ="Doe",  SocialSecurityNumber="6575-574-6544" });
    //     response.EnsureSuccessStatusCode();
    // }

    [Fact]
    public async Task UpdateEmployee_ReturnNotFoundForNoneExistantEmployees() {
        var client = _factory.CreateClient();
        var response = await client.PutAsJsonAsync("/employees/99999", new Employee { FirstName = "John", LastName =  "Doe",   SocialSecurityNumber="6575-574-6544" });
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
} 


/*
    Assert.True(response.IsSuccessStatusCode)
    can also work but prefered response.EnsureSuccessStatusCode()
*/
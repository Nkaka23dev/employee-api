using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

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
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/employees", new {});
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
     public async Task UpdateEmployee_ReturnOkResults() {
        var client = _factory.CreateClient();
        var response = await client.PutAsJsonAsync("/employees/2", new Employee { FirstName = "John", LastName ="Doe",  SocialSecurityNumber="6575-574-6544" });
        response.EnsureSuccessStatusCode();
    }

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
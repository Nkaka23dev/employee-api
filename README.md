
  ### About the project

- An `ASP.NET Core` employee project. Under the hood(C# and .NET), the  project uses `SQLite` database which can be changed in production.

#### Getting started:
```ts
 - Clone the project
 - You need to have `dotnet` and `ef` installed
 ```

#### Useful commands:
```ts
 - dotnet build
 - dotnet test
 - dotnet run
 - dotnet ef database update
 - dotnet ef migrations add init
 - dotnet ef migrations add --file-name
 ```
#### Filtering and Pagination example:
```ts
 http://localhost:5229/employee/all?page=2&RequestPerPage=3&FirstNameContains={firstname}&LastNameContains={lastname}
 ```
- Unit test included using  
```bash
 - XUnit
 - Microsoft.AspNetCore.OpenApi Version=9.0.2
 ```
 - API endpoints are validated using FluentValidation
 ```bash
   -  dotnet add package FluentValidation
 ```
 ```ts
    You can also use NuGet to add this package throught the VScode UI.
 ```
 ```ts
    Use AutoMapper to similate object.assing()
   ```
- To including ILogger mocks while testing we can mock ILogger interface. You can use one of the following libries: 
```bash
 - Moq
 - ILogger.Moq
 - ILogger.Log
 ```

### Used pattern

- Repository Pattern

 
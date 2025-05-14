## Employee API  

This is an **ASP.NET Core** employee management system built with **C#** and **.NET**. The project currently uses an **SQLite** database for development, but it can be switched to **Postgres** for production environments. It  will provides basic employee management features such as handling employee benefits, leave requests, performance reviews, and payroll.

## Already Implemented Features✅

<img width="1055" alt="Screenshot 2025-04-09 at 20 22 04" src="https://github.com/user-attachments/assets/a2de64ff-5b14-4b2d-be4f-f9f89d78d67b" />

### Good To know

- **Data Seeding**: when the project starts two employees are added by default.
- **Get Employees with filtering and pagination**: Endpoint to retrieve employees with support for:
  - **Pagination**: Get a specific number of employees per page.
  - **Filtering**: Filter employees based on various criteria (e.g., first name, last name, etc..).

- **Employee Validation**: Validation of employee data on creation and update to ensure data integrity.
- **Logging**: Comprehensive logging of actions such as employee creation, updates, and deletions for auditing and debugging purposes.
- **Unit Testing**: Only on employees' features so far
- **API Documentation**: Automatically generated Swagger documentation for all employee-related endpoints, providing detailed information about availables

## TODO  🚀


- ✅ Implement(with Access and Refresh tokens) **authentication and authorization**[Done on User endpoints!]
- You can start by improving already implemented features:
  1. ✅Repostory pattern(creating interfaces and separating database and business logic)
  2. ✅Code clean-up
  3. ✅Seeding two users with their email and password and updating READMe wiht login credentials
  4. Users can have roles and roles determine what users can do in the app
  5. Add Validation throughout
using **AutoMapper** in the **EmployeeController**
- Data seeding for at least two users to begin with and document the it in README file
- Protect employee endpoints and update tests
- Update user endpoint is not working as expected; it should be fixed.
- Migrate from **SQLite** to **Postgres** for production.  
- Implement **CRUD operations** for employee benefits (**[Table reference](https://github.com/Nkaka23dev/employee-api/blob/2070366409f04be52f8d7528011508a6831ea5f7/TheEmployeeAPI/Domain/Entities/Employee/Employee.cs#L25)**).  
- Add **CI/CD pipelines** for automated deployment(Optional include versioning with semantic release)  
- **Dockerize** the API for containerized deployment.    
- Allow employees to **upload documents**, such as IDs and images.  
- Implement **account verification** via email and **Google login**.  
-  **Write unit tests** for every newly added feature to ensure code quality.

## More advance Todo Features 🎯  

The API now should include:  
- **Leave Requests**: Allow employees to submit leave requests.  
- **Performance Reviews**: Track and manage performance evaluations for employees.  
- **Payroll Management**: Record and manage payroll details.  
- **Training Management**: Manage training programs and employee enrollments.  
- **Time Tracking**: Track employee working hours.  
- **User Roles**: Role-based access for admins, managers, and employees.


## Proposed Entity Descriptions 🗂  

- **Employees**: The core entity; nearly all other entities are related to employees.  
- **EmployeeBenefits**: Each employee can have multiple associated benefits.  
- **LeaveRequests**: Employees can request leave, and each employee can hsave multiple leave requests.  
- **PerformanceReviews**: Employees can have multiple performance reviews over time.  
- **Payroll**: Each employee has one payroll record per payment period.  
- **Trainings & EmployeeTrainings**: Employees can enroll in multiple training programs.  
- **TimeTracking**: Tracks employee clock-in and clock-out times.  
- **UserRoles & EmployeeRoles**: Defines roles such as admin, manager, and employee to manage access and permissions.

## Getting Started 🛠  

### Prerequisites  
Ensure you have the following installed:  s
- **.NET SDK** (version 9.0 or later)  
- **Entity Framework (EF Core)**  
- **PostgreSQL** (optional, if migrating from SQLite)
- **NuGet Gallery** VSCode extention(Optional)

### Setup Instructions  
```sh
# Clone the repository
git clone https://github.com/Nkaka23dev/employee-api.git

# Navigate to the project directory
cd employee-api

# Restore the required packages
dotnet restore

# Build the project
dotnet build

# Run project tests
dotnet test

# Navigate to TheEmployeeAPI
cd TheEmployeeAPI

# Run the project locally
dotnet run
```

## Setting Up the Local Environment

1. Copy the `appsettings.example.json` file to `appsettings.json`.
   

#### Useful commands:
```ts
 - dotnet build //Build the project
 - dotnet test //Running tests
 - dotnet run  //Starting the project
 - dotnet ef migrations add --file-name //Creating new migration
 - dotnet ef database update //Modified database with new migration

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

 ## Contributing

We welcome your contributions to this learning project! 

### Used pattern

- Repository Pattern

## Employee API  

This is an **ASP.NET Core** employee management system built with **C#** and **.NET**. The project currently uses **Postgres** database.  It includes different services, such as a standard RESTful API (JSON) and a SOAP service.

[Click here for the frontend built with Angular.](https://github.com/Nkaka23dev/employee-panel)

## Getting Started 🛠

#### Prerequisites  

Ensure you have the following installed:  
- **.NET SDK**: (version 9.0 or later)  
- **Entity Framework (EF Core)**  
- **PostgreSQL**: database
- **NuGet Gallery**: VSCode extention(Optional)
- **dotnet-svcutil**: A tool to generate WCF SOAP client proxies for consuming SOAP services. [Link](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/dotnet-svcutil-guide?tabs=dotnetsvcutil2x).
- **SoapUI**: GUI tool for testing SOAP and REST APIs

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

# (Optional) Run project tests
dotnet test
```

### SOAP Service - Setup:
```sh
# Navigate to BenefitSoapService
cd BenefitSoapService

# Run the project locally
dotnet run

# Hot Rerun
dotnet watch run

# Visit for WSDL files
http://localhost:5079/BenefitService.svc

# Run the following to generate C# proxy classes from the WSDL:
dotnet-svcutil http://localhost:5079/BenefitService.svc?singleWsdl -n "*,BenefitSoapService.Client"
```
### Test BenefitSoapService with SoapUI

After successfully running the project, copy the WSDL URL from the browser and paste it into the [SoapUI tool](https://www.soapui.org/) 
to explore all available services and make test requests.

You can also copy the request environment from SoapUI and use it in **Postman** or any frontend UI to make calls to the service.

What you might expect after successfully running BenefitSoapService

<img width="1672" alt="Screenshot 2025-05-23 at 15 35 14" src="https://github.com/user-attachments/assets/7db0eeaa-284d-4a4c-a5ee-d175381da413" />
<img width="1626" alt="Screenshot 2025-05-23 at 17 18 06" src="https://github.com/user-attachments/assets/15980369-63cc-4fe6-9ace-51887b724021" />

Request to get Benefits from Postman

<img width="1268" alt="Screenshot 2025-05-23 at 15 36 00" src="https://github.com/user-attachments/assets/712c8c6c-7c0b-4407-b711-2f54dc324dbe" />

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

## TODO NEXT

 1. Consider using SOAP protocol (likely for integration with legacy systems or enterprise services). Ensure your service is SOAP-compliant where required.
 2. If using SOAP, you might need to integrate with a different service (not the current one). Clarify which service needs SOAP and design accordingly.

- More suggestion: 
 1. Consider using Kafka for communication between the department service and the employee API, to ensure asynchronous, decoupled microservice interaction.
 2. Think about concurrency control: What should happen if two users attempt to delete the same resource at the same time? Implement appropriate safeguards (e.g., optimistic locking, idempotency, or conflict detection)

- Update user endpoint is not working as expected; it should be fixed.  
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
   
## Login Credentials 
<b>Email</b>
 ```bash
 eric.nkaka@example.com 
 ```
<b>Password</b>
 ```bash
 Password123! 
 ```

## Filtering and Pagination 
```ts
 http://localhost:5229/employee/all?page=2&RequestPerPage=3&FirstNameContains={firstname}&LastNameContains={lastname}
 ```

## Useful commands:
```ts
 - dotnet build //Build the project
 - dotnet test //Running tests
 - dotnet run  //Starting the project
 - dotnet ef migrations add --file-name //Creating new migration
 - dotnet ef database update //Modified database with new migration

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

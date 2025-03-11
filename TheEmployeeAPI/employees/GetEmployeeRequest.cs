namespace TheEmployeeAPI.employees;

public class GetEmployeeResponse{
    public  required string FirstName { get; set; }
    public  required string LastName {get; set;}
    public  string? Address1 {get; set;}
    public  string? Address2 {get; set;}
    public  string? City {get; set;}
    public  string? State {get; set;}  
    public  string? ZipCode {get; set;}
    public  string? PhoneNumber {get; set;}
    public  string? Email {get; set;}
    public required List<GetEmployeeResponseEmployeeBenefits> Benefits {get;set;}

} 

public class GetEmployeeResponseEmployeeBenefits{
    public int Id {get; set;}
    public int EmployeeId {get; set;}
    public BenefitsType BenefitsType {get; set;}
    public decimal Cost {get; set;}
}
 
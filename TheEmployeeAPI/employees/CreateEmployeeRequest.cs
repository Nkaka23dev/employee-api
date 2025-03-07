using FluentValidation;

namespace TheEmployeeAPI.employees;

public class CreateEmployeeRequest
{
    public string? FirstName { get; set; }
    public string? LastName {get; set;}
    public string? SocialSecurityNumber {get; set;}
    public  string? Address1 {get; set;}
    public  string? Address2 {get; set;} 
    public  string? City {get; set;}
    public  string? State {get; set;}   
    public  string? ZipCode {get; set;}
    public  string? PhoneNumber {get; set;}
    public  string? Email {get; set;}
}

public class CreateEmployeeRequestValidator: AbstractValidator<CreateEmployeeRequest>{
    public CreateEmployeeRequestValidator(){
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Should provide 'First Name'");
        RuleFor(x => x.LastName).NotEmpty();
    } 
}
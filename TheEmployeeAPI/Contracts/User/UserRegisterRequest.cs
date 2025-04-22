namespace TheEmployeeAPI.Contracts.User;

public class UserRegisterRequest
{
    public required string FirstName {get; set;}
    public required string LastName {get; set;}
    public string? Email {get; set;}
    public required string Password {get;set;}
    public string? Gender {get; set;}
}

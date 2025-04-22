namespace TheEmployeeAPI.Contracts.User;

public class UserResponse
{
    public Guid Id {get; set;}
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
    public string? Email {get; set;}
    public string? Gender {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;} 
    public string? AccessToken {get; set;}
    public string? RefreshToken {get; set;}
}

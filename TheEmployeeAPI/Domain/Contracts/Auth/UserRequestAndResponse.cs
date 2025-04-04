namespace TheEmployeeAPI.Domain.Contracts.Auth;

public class UserRegisterRequest 
{
    public required string FirstName {get; set;}
    public required string LastName {get; set;}
    public string? Email {get; set;}
    public required string Password {get;set;}
    public string? Gender {get; set;}

}

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

public class UserLoginRequest
 {
    public string? Email {get; set;}
    public string? password {get; set;}
}

public class CurrentUserResponse
 {
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
    public string? Email {get; set;}
    public string? Gender {get; set;}
    public DateTime CreatedAt {get; set;} 
    public DateTime UpdatedAt {get; set;} 
    public string? AccessToken {get; set;}
}

public class UpdatedUserRequest
 {
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
    public string? Email {get; set;}
    public string? Password {get; set;}
    public string? Gender {get; set;}
}

public class RevokeRefreshTokenResponse 
{
    public string? Token {set; get;}
}

public class RefreshTokenRequest 
{
    public string? RefreshToken {set; get;}
}
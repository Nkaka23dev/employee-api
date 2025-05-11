namespace TheEmployeeAPI.Contracts.User;

public class UserLoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

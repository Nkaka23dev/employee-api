namespace TheEmployeeAPI.Domain.DTOs.Authentication
{
    public class RegisterRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
        public string? Gender { get; set; }
        public required string Role {get; set;}
    }
}

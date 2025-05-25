using FluentValidation;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace TheEmployeeAPI.Domain.DTOs.Authentication
{
    public class RegisterRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Gender { get; set; }
        public required string Role { get; set; }
    }

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IOptions<IdentityOptions> identityOptions)
        {
            var passwordOptions = identityOptions.Value.Password;

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email format is invalid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(passwordOptions.RequiredLength)
                .WithMessage($"Password must be at least {passwordOptions.RequiredLength} characters long.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(r => UserRoles.All.Contains(r, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Invalid role. Allowed roles are: Admin, Manager, Employee.");

        }
    }
}

using Core.Domain.DTOs;
using FluentValidation;

namespace TheEmployeeAPI.Domain.DTOs.Employees
{
    public class CreateEmployeeRequest: IEmployeeBenefitRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string SocialSecurityNumber { get; set; }
        public required string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public List<int> BenefitsIds { get; set; } = [];
    }

    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.SocialSecurityNumber)
                .NotEmpty().WithMessage("Social Security Number is required.")
                .Matches(@"^\d{3}-\d{2}-\d{4}$")
                .WithMessage("Social Security Number must be in the format XXX-XX-XXXX.");

            RuleFor(x => x.Address1)
                .NotEmpty().WithMessage("Address1 is required.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip Code is required.")
                .Matches(@"^\d{5}(-\d{4})?$")
                .WithMessage("Zip Code must be 5 digits or in the format 12345-6789.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^(?:\+250|0)7\d{8}$")
                .WithMessage("Phone number must be a valid Rwanda phone number.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");
        }
    }
}

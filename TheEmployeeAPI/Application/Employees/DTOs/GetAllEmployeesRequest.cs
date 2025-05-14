using FluentValidation;

namespace TheEmployeeAPI.Application.Employees.DTOs
{
    public class GetAllEmployeesRequest
    {
        public int? Page { get; set; }
        public int? RequestPerPage { get; set; }
        public string? FirstNameContains { get; set; }
        public string? LastNameContains { get; set; }
    }

    public class GetAllEmployeesRequestValidator : AbstractValidator<GetAllEmployeesRequest>
    {
        public GetAllEmployeesRequestValidator()
        {
            RuleFor(r => r.Page).GreaterThanOrEqualTo(1).WithMessage("Page must be set to non-zero positive number.");
            RuleFor(r => r.RequestPerPage)
            .GreaterThanOrEqualTo(1).WithMessage("You must return atleast one record.")
            .LessThanOrEqualTo(100).WithMessage("You cannot return more than 100 records.");

        }
    }
}
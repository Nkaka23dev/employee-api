using FluentValidation;

namespace TheEmployeeAPI.Domain.DTOs.Authentication
{
    public class RefreshTokenRequest
    {
        public required string RefreshToken { set; get; }

    }

    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
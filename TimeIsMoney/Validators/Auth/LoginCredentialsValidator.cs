using FluentValidation;
using TimeIsMoney.Api.Validators.BaseValidators;
using TimeIsMoney.Services.Auth.Models;

namespace TimeIsMoney.Api.Validators.Auth
{
    public class LoginCredentialsValidator : AbstractValidator<LoginCredentialsModel>
    {
        public LoginCredentialsValidator()
        {
            RuleFor(x => x.GrantType)
                .Equal("password")
                .WithMessage("Invalid grant type");

            RuleFor(x => x.Password)
                .SetValidator(new PasswordValidator());

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email address");
        }
    }
}
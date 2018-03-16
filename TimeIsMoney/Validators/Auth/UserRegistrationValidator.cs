using FluentValidation;
using TimeIsMoney.Api.Validators.BaseValidators;
using TimeIsMoney.Services.Auth.Models;

namespace TimeIsMoney.Api.Validators.Auth
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationModel>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email is not valid");

            RuleFor(x => x.Password)
                .SetValidator(new PasswordValidator());
        }
    }
}
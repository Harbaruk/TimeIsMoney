using FluentValidation;

namespace TimeIsMoney.Api.Validators.BaseValidators
{
    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            RuleFor(x => x)
                .MinimumLength(5)
                .WithMessage("Should be at least 5 characters");
        }
    }
}
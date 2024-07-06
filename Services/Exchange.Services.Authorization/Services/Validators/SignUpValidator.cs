using Exchange.Services.Authorization.Data.DTO;
using FluentValidation;

namespace Exchange.Services.Authorization.Services.Validators;

public class SignUpValidator : AbstractValidator<SignUpDTO>
{
    public SignUpValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(50).WithMessage("Password is long.");
    }
}

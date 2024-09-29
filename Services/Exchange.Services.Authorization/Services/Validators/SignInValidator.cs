using Exchange.Services.Authorization.Data.DTO;
using FluentValidation;

namespace Exchange.Services.Authorization.Services.Validators;

public class SignInValidator : AbstractValidator<SignInDto>
{
    public SignInValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Empty email");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Empty password");
    }
}

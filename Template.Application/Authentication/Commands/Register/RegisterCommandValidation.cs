using FluentValidation;
using Template.Application.Common.Patterns;

namespace Template.Application.Authentication.Commands.Register;

public class RegisterCommandValidation
    : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.FirstName).NotEmpty().Length(2, 25);
        RuleFor(x => x.LastName).NotEmpty().Length(2, 30);
        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(6, 50)
            .Matches(Patterns.Auth.Password);
    }
}

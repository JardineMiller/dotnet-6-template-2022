using FluentValidation;
using Template.Application.Common.Patterns;

namespace Template.Application.Authentication.Commands.Register;

public class RegisterCommandValidation
    : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.FirstName).Length(2, 25);
        RuleFor(x => x.LastName).Length(2, 30);
        RuleFor(x => x.Password)
            .Length(6, 16)
            .Matches(Patterns.Auth.Password);
    }
}

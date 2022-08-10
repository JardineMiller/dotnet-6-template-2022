using FluentValidation;
using Template.Application.Authentication.Commands.Register;

namespace Template.Api.Validation;

public class RegisterCommandValidation
    : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).Length(6, 16);
        RuleFor(x => x.FirstName).Length(2, 25);
        RuleFor(x => x.LastName).Length(2, 30);
    }
}
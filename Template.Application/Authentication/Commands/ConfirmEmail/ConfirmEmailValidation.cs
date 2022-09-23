using FluentValidation;

namespace Template.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailValidation
    : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Token).NotEmpty();
    }
}

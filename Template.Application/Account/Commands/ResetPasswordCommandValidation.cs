using FluentValidation;
using Template.Application.Common.Patterns;

namespace Template.Application.Account.Commands;

public class ResetPasswordCommandValidation
    : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Length(6, 50)
            .Matches(Patterns.Auth.Password);

        RuleFor(x => x.NewPassword)
            .NotEqual(x => x.OldPassword)
            .When(x => !string.IsNullOrEmpty(x.OldPassword));

        RuleFor(x => x.Token)
            .NotEmpty()
            .When(x => string.IsNullOrEmpty(x.OldPassword));

        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .Length(6, 50)
            .Matches(Patterns.Auth.Password)
            .When(x => string.IsNullOrEmpty(x.Token));

        When(
            x =>
                !string.IsNullOrEmpty(x.Token)
                && !string.IsNullOrEmpty(x.OldPassword),
            () =>
            {
                RuleFor(x => x.Token).Null();
                RuleFor(x => x.OldPassword).Null();
            }
        );
    }
}

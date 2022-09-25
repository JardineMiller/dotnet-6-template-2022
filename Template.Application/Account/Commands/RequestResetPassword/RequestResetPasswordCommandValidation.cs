using FluentValidation;

namespace Template.Application.Account.Commands.RequestResetPassword;

public class RequestResetPasswordCommandValidation
    : AbstractValidator<RequestResetPasswordCommand>
{
    public RequestResetPasswordCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}

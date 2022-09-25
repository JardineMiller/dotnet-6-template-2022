using ErrorOr;
using MediatR;
using Template.Application.Account.Common;

namespace Template.Application.Account.Commands.ResetPassword;

public record ResetPasswordCommand(
    string Email,
    string NewPassword,
    string? Token = null,
    string? OldPassword = null
) : IRequest<ErrorOr<ResetPasswordResult>>;

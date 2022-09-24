using MediatR;
using ErrorOr;
using Template.Application.Account.Common;

namespace Template.Application.Account.Commands;

public record ResetPasswordCommand(
    string Email,
    string NewPassword,
    string? Token = null,
    string? OldPassword = null
) : IRequest<ErrorOr<ResetPasswordResult>>
{
    // public bool IsTokenReset => Token is not null;
}

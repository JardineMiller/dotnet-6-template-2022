using MediatR;
using ErrorOr;
using Template.Application.Account.Common;

namespace Template.Application.Account.Commands.RequestResetPassword;

public record RequestResetPasswordCommand(string Email)
    : IRequest<ErrorOr<RequestResetPasswordResult>>;

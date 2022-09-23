using MediatR;
using ErrorOr;
using Template.Application.Authentication.Common;

namespace Template.Application.Authentication.Commands.ConfirmEmail;

public record ConfirmEmailCommand(string Email, string Token)
    : IRequest<ErrorOr<AuthenticationResult>>;

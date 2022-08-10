using ErrorOr;
using MediatR;
using Template.Application.Authentication.Common;

namespace Template.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;
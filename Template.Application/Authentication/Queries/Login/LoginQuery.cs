using ErrorOr;
using MediatR;
using Template.Application.Authentication.Common;

namespace Template.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password)
    : IRequest<ErrorOr<AuthenticationResult>>;

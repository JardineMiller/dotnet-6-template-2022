using ErrorOr;
using MediatR;
using Template.Application.Authentication.Common;
using Template.Application.Common.Interfaces.Authentication;
using Template.Application.Common.Interfaces.Persistence;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;

namespace Template.Application.Authentication.Queries.Login;

public class LoginQueryHandler
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(
        IUserRepository userRepository,
        IJwtGenerator jwtGenerator
    )
    {
        this._userRepository = userRepository;
        this._jwtGenerator = jwtGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery qry,
        CancellationToken cancellationToken
    )
    {
        if (
            this._userRepository.GetUserByEmail(qry.Email)
            is not User user
        )
        {
            return Errors.Authentication.InvalidCredentials;
        }

        if (user.Password != qry.Password)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token = this._jwtGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}

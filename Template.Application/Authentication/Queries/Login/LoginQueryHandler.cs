using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Template.Application.Authentication.Common;
using Template.Application.Common.Interfaces.Authentication;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;

namespace Template.Application.Authentication.Queries.Login;

public class LoginQueryHandler
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtGenerator _jwtGenerator;

    public LoginQueryHandler(
        IJwtGenerator jwtGenerator,
        UserManager<User> userManager
    )
    {
        this._jwtGenerator = jwtGenerator;
        this._userManager = userManager;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery qry,
        CancellationToken cancellationToken
    )
    {
        if (
            await this._userManager.FindByEmailAsync(qry.Email)
            is not User user
        )
        {
            return Errors.Authentication.InvalidCredentials;
        }

        if (
            !await this._userManager.CheckPasswordAsync(
                user,
                qry.Password
            )
        )
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token = this._jwtGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}

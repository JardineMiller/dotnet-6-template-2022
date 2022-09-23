using MediatR;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Template.Application.Authentication.Common;
using Template.Application.Common.Interfaces.Authentication;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;

namespace Template.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler
    : IRequestHandler<
          ConfirmEmailCommand,
          ErrorOr<AuthenticationResult>
      >
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtGenerator _jwtGenerator;

    public ConfirmEmailCommandHandler(
        UserManager<User> userManager,
        IJwtGenerator jwtGenerator
    )
    {
        this._userManager = userManager;
        this._jwtGenerator = jwtGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        ConfirmEmailCommand cmd,
        CancellationToken cancellationToken
    )
    {
        if (
            await this._userManager.FindByEmailAsync(cmd.Email)
            is not User user
        )
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var result = await this._userManager.ConfirmEmailAsync(
            user,
            cmd.Token
        );

        if (!result.Succeeded)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token = this._jwtGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}

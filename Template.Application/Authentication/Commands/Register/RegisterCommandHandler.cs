using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Template.Application.Authentication.Common;
using Template.Application.Common.Interfaces.Authentication;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;

namespace Template.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtGenerator _jwtGenerator;
    private readonly UserManager<User> _userManager;

    public RegisterCommandHandler(
        IJwtGenerator jwtGenerator,
        UserManager<User> userManager
    )
    {
        this._jwtGenerator = jwtGenerator;
        this._userManager = userManager;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand cmd,
        CancellationToken cancellationToken
    )
    {
        // Check if user already exists
        if (
            await this._userManager.FindByEmailAsync(cmd.Email)
            is not null
        )
        {
            return Errors.User.DuplicateEmail;
        }

        // Create user (generate unique id)
        var user = new User
        {
            FirstName = cmd.FirstName,
            LastName = cmd.LastName,
            Email = cmd.Email,
            UserName = cmd.Email,
        };

        var result = await this._userManager.CreateAsync(
            user,
            cmd.Password
        );

        if (!result.Succeeded)
        {
            return Errors.User.CreationFailed;
        }

        // Create JWT
        var token = this._jwtGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}

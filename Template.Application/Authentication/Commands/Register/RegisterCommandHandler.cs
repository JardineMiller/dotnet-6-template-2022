using ErrorOr;
using MediatR;
using Template.Application.Authentication.Common;
using Template.Application.Common.Interfaces.Authentication;
using Template.Application.Common.Interfaces.Persistence;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;

namespace Template.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IJwtGenerator jwtGenerator
    )
    {
        this._userRepository = userRepository;
        this._jwtGenerator = jwtGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand cmd,
        CancellationToken cancellationToken
    )
    {
        // Check if user already exists
        if (
            this._userRepository.GetUserByEmail(cmd.Email) is not null
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
            Password = cmd.Password
        };

        this._userRepository.Add(user);

        // Create JWT
        var token = this._jwtGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
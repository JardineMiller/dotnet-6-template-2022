using System.Linq;
using System.Threading;
using Moq;
using Shouldly;
using Template.Application.Authentication.Commands.Register;
using Template.Application.Common.Interfaces.Authentication;
using Template.Application.Common.Interfaces.Persistence;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;
using Template.Infrastructure.Persistence;
using Xunit;

namespace Template.Application.Tests.Application.Tests.Authentication.Commands.Register;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IJwtGenerator> _jwtGeneratorMock;

    private readonly IUserRepository _userRepository =
        new UserRepository();

    private const string validFirstName = "Test";
    private const string validLastName = "User";
    private const string validEmail = "test2@email.com";
    private const string validPassword = "Password123!";

    public RegisterCommandHandlerTests()
    {
        this._jwtGeneratorMock = new Mock<IJwtGenerator>();
        this._jwtGeneratorMock
            .Setup(j => j.GenerateToken(It.IsAny<User>()))
            .Returns("token");
    }

    private void Teardown()
    {
        this._userRepository.Clear();
    }

    [Fact]
    public void Handle_GivenValidRequest_ReturnsCorrectResponse()
    {
        // Arrange
        var command = new RegisterCommand(
            validFirstName,
            validLastName,
            validEmail,
            validPassword
        );

        // Act
        var handler = new RegisterCommandHandler(
            new UserRepository(),
            this._jwtGeneratorMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.Value.Token.ShouldBe("token");
        result.Value.User.FirstName.ShouldBe(validFirstName);
        result.Value.User.LastName.ShouldBe(validLastName);
        result.Value.User.Email.ShouldBe(validEmail);

        Teardown();
    }

    [Fact]
    public void Handle_GivenExistingUserEmail_ShouldReturnUserAuthError()
    {
        // Arrange
        var command = new RegisterCommand(
            validFirstName,
            validLastName,
            validEmail,
            validPassword
        );

        var command2 = new RegisterCommand(
            validFirstName,
            validLastName,
            validEmail,
            validPassword
        );

        // Act
        var handler = new RegisterCommandHandler(
            this._userRepository,
            this._jwtGeneratorMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        var result2 = handler
            .Handle(command2, CancellationToken.None)
            .Result;

        // Assert
        result2.Errors.Count.ShouldBe(1);
        result2.Errors
            .First()
            .Code.ShouldBe(Errors.User.DuplicateEmail.Code);
        result2.Errors
            .First()
            .Description.ShouldBe(
                Errors.User.DuplicateEmail.Description
            );

        Teardown();
    }
}

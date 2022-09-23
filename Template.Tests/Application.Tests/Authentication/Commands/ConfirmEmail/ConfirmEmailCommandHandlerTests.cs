using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;
using Template.Application.Authentication.Commands.ConfirmEmail;
using Template.Application.Common.Interfaces.Authentication;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;
using Xunit;

namespace Template.Application.Tests.Application.Tests.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandlerTests
{
    private Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IJwtGenerator> _jwtGeneratorMock = new();

    private const string validEmail = "test2@email.com";
    private const string validToken = "tokens-are-awesome";
    private User validUser =
        new()
        {
            FirstName = "Test",
            LastName = "User",
            Email = validEmail
        };

    public ConfirmEmailCommandHandlerTests()
    {
        this._userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );
    }

    [Fact]
    public void Handle_GivenNonExistingEmail_ReturnsInvalidCredError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(null as User);

        var command = new ConfirmEmailCommand(validEmail, validToken);
        var handler = new ConfirmEmailCommandHandler(
            this._userManagerMock.Object,
            this._jwtGeneratorMock.Object
        );

        // Act
        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .First()
            .Code.ShouldBe(
                Errors.Authentication.InvalidCredentials.Code
            );
        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Authentication.InvalidCredentials.Description
            );
    }

    [Fact]
    public void Handle_GivenInvalidEmailAndToken_ReturnsInvalidCredError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(this.validUser);

        this._userManagerMock
            .Setup(
                x =>
                    x.ConfirmEmailAsync(
                        It.IsAny<User>(),
                        It.IsAny<string>()
                    )
            )!
            .ReturnsAsync(IdentityResult.Failed());

        var command = new ConfirmEmailCommand(validEmail, validToken);
        var handler = new ConfirmEmailCommandHandler(
            this._userManagerMock.Object,
            this._jwtGeneratorMock.Object
        );

        // Act
        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .First()
            .Code.ShouldBe(
                Errors.Authentication.InvalidCredentials.Code
            );
        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Authentication.InvalidCredentials.Description
            );
    }

    [Fact]
    public void Handle_GivenValidInput_ReturnsValidOutput()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(this.validUser);

        this._userManagerMock
            .Setup(
                x =>
                    x.ConfirmEmailAsync(
                        It.IsAny<User>(),
                        It.IsAny<string>()
                    )
            )!
            .ReturnsAsync(IdentityResult.Success);

        this._jwtGeneratorMock
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns("token");

        var command = new ConfirmEmailCommand(validEmail, validToken);
        var handler = new ConfirmEmailCommandHandler(
            this._userManagerMock.Object,
            this._jwtGeneratorMock.Object
        );

        // Act
        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.Value.Token.ShouldBe("token");
        result.Value.User.FirstName.ShouldBe(
            this.validUser.FirstName
        );
        result.Value.User.LastName.ShouldBe(this.validUser.LastName);
        result.Value.User.Email.ShouldBe(this.validUser.Email);
    }
}

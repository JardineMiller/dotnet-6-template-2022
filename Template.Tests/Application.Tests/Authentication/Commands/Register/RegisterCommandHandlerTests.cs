using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;
using Template.Application.Authentication.Commands.Register;
using Template.Application.Common.Interfaces.Authentication;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;
using Xunit;

namespace Template.Application.Tests.Application.Tests.Authentication.Commands.Register;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IJwtGenerator> _jwtGeneratorMock;
    private readonly Mock<UserManager<User>> _userManagerMock;

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
    public void Handle_GivenValidRequest_ReturnsCorrectResponse()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(null as User);

        this._userManagerMock
            .Setup(
                x =>
                    x.CreateAsync(
                        It.IsAny<User>(),
                        It.IsAny<string>()
                    )
            )
            .ReturnsAsync(IdentityResult.Success);

        var command = new RegisterCommand(
            validFirstName,
            validLastName,
            validEmail,
            validPassword
        );

        // Act
        var handler = new RegisterCommandHandler(
            this._jwtGeneratorMock.Object,
            this._userManagerMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.Value.Token.ShouldBe("token");
        result.Value.User.FirstName.ShouldBe(validFirstName);
        result.Value.User.LastName.ShouldBe(validLastName);
        result.Value.User.Email.ShouldBe(validEmail);
    }

    [Fact]
    public void Handle_GivenExistingUserEmail_ShouldReturnUserAuthError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(new User() { Email = validEmail });

        var command = new RegisterCommand(
            validFirstName,
            validLastName,
            validEmail,
            validPassword
        );

        // Act
        var handler = new RegisterCommandHandler(
            this._jwtGeneratorMock.Object,
            this._userManagerMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .First()
            .Code.ShouldBe(Errors.User.DuplicateEmail.Code);
        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.User.DuplicateEmail.Description
            );
    }
}

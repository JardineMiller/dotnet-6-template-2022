using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;
using Template.Application.Authentication.Queries.Login;
using Template.Application.Common.Interfaces.Authentication;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;
using Xunit;

namespace Template.Application.Tests.Application.Tests.Authentication.Queries;

public class LoginQueryHandlerTests
{
    private readonly Mock<IJwtGenerator> _jwtGeneratorMock;
    private readonly Mock<UserManager<User>> _userManagerMock;

    private const string validFirstName = "Test";
    private const string validLastName = "User";
    private const string validEmail = "test1@user.com";
    private const string validPassword = "Password123!";

    private const string invalidEmail = "doesnt@exist.com";
    private const string invalidPassword = "IncorrectPassword!";

    public LoginQueryHandlerTests()
    {
        this._jwtGeneratorMock = new Mock<IJwtGenerator>();
        this._jwtGeneratorMock
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
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
    public async Task Handle_GivenNonExistingUser_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(null as User);

        var query = new LoginQuery(invalidEmail, validPassword);
        var handler = new LoginQueryHandler(
            this._jwtGeneratorMock.Object,
            this._userManagerMock.Object
        );

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

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
    public async Task Handle_GivenIncorrectPassword_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(
                new User()
                {
                    FirstName = validFirstName,
                    LastName = validLastName,
                    Email = validEmail,
                    EmailConfirmed = true
                }
            );

        this._userManagerMock
            .Setup(
                x =>
                    x.CheckPasswordAsync(
                        It.IsAny<User>(),
                        invalidPassword
                    )
            )!
            .ReturnsAsync(false);

        var query = new LoginQuery(validEmail, invalidPassword);
        var handler = new LoginQueryHandler(
            this._jwtGeneratorMock.Object,
            this._userManagerMock.Object
        );

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

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
    public async Task Handle_GivenUserHasNotConfirmedEmail_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(
                new User()
                {
                    FirstName = validFirstName,
                    LastName = validLastName,
                    Email = validEmail
                }
            );

        this._userManagerMock
            .Setup(
                x =>
                    x.CheckPasswordAsync(
                        It.IsAny<User>(),
                        invalidPassword
                    )
            )!
            .ReturnsAsync(false);

        var query = new LoginQuery(validEmail, invalidPassword);
        var handler = new LoginQueryHandler(
            this._jwtGeneratorMock.Object,
            this._userManagerMock.Object
        );

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .First()
            .Code.ShouldBe(
                Errors.Authentication.EmailNotConfirmed.Code
            );
        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Authentication.EmailNotConfirmed.Description
            );
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsCorrectResponse()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(
                new User()
                {
                    FirstName = validFirstName,
                    LastName = validLastName,
                    Email = validEmail,
                    EmailConfirmed = true
                }
            );

        this._userManagerMock
            .Setup(
                x =>
                    x.CheckPasswordAsync(
                        It.IsAny<User>(),
                        validPassword
                    )
            )!
            .ReturnsAsync(true);

        var query = new LoginQuery(validEmail, validPassword);
        var handler = new LoginQueryHandler(
            this._jwtGeneratorMock.Object,
            this._userManagerMock.Object
        );

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        result.Value.Token.ShouldBe("token");
        result.Value.User.FirstName.ShouldBe(validFirstName);
        result.Value.User.LastName.ShouldBe(validLastName);
        result.Value.User.Email.ShouldBe(validEmail);
    }
}

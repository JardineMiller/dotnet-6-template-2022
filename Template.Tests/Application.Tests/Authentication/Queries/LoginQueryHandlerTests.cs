using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using Template.Application.Authentication.Queries.Login;
using Template.Application.Common.Interfaces.Authentication;
using Template.Application.Common.Interfaces.Persistence;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;
using Template.Infrastructure.Persistence;
using Xunit;

namespace Template.Application.Tests.Application.Tests.Authentication.Queries;

public class LoginQueryHandlerTests
{
    private readonly Mock<IJwtGenerator> _jwtGeneratorMock;

    private readonly IUserRepository _userRepository =
        new UserRepository();

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
    }

    private void Setup()
    {
        this._userRepository.Add(
            new User()
            {
                FirstName = validFirstName,
                LastName = validLastName,
                Email = validEmail,
            }
        );
    }

    private void Teardown()
    {
        this._userRepository.Clear();
    }

    [Fact]
    public async Task Handle_GivenNonExistingUser_ReturnsError()
    {
        Setup();

        // Arrange
        var query = new LoginQuery(invalidEmail, validPassword);
        var handler = new LoginQueryHandler(
            this._userRepository,
            this._jwtGeneratorMock.Object
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

        Teardown();
    }

    [Fact]
    public async Task Handle_GivenIncorrectPassword_ReturnsError()
    {
        Setup();

        // Arrange
        var query = new LoginQuery(validEmail, invalidPassword);
        var handler = new LoginQueryHandler(
            this._userRepository,
            this._jwtGeneratorMock.Object
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

        Teardown();
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsCorrectResponse()
    {
        Setup();

        // Arrange
        var query = new LoginQuery(validEmail, validPassword);
        var handler = new LoginQueryHandler(
            this._userRepository,
            this._jwtGeneratorMock.Object
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

        Teardown();
    }
}

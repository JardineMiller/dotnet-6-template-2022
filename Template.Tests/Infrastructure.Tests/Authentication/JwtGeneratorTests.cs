using System;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Template.Application.Common.Interfaces.Services;
using Template.Domain.Entities;
using Template.Infrastructure.Authentication;
using Xunit;

namespace Template.Application.Tests.Infrastructure.Tests.Authentication;

public class JwtGeneratorTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock;

    private readonly User _user1 =
        new()
        {
            FirstName = "test",
            LastName = "user1",
            Password = "password123!",
            Email = "test@user1.com"
        };

    public JwtGeneratorTests()
    {
        this._dateTimeProviderMock = new Mock<IDateTimeProvider>();
        this._dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(new DateTime(2023, 1, 1));

        this._jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
        this._jwtSettingsMock
            .Setup(x => x.Value)
            .Returns(
                new JwtSettings
                {
                    Secret = "super-secret-secret",
                    Issuer = "issuer",
                    Audience = "audience",
                    ExpiryMinutes = 60
                }
            );
    }

    [Fact]
    public void GenerateToken_GivenValidInput_ProvidesAToken()
    {
        // Arrange
        var generator = new JwtGenerator(
            this._dateTimeProviderMock.Object,
            this._jwtSettingsMock.Object
        );

        // Act
        var token1 = generator.GenerateToken(this._user1);

        // Assert
        token1.ShouldNotBeNull();
    }
}

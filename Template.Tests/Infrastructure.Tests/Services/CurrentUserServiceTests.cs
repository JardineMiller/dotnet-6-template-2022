using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using Template.Infrastructure.Services;
using Xunit;

namespace Template.Application.Tests.Infrastructure.Tests.Services;

public class CurrentUserServiceTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock =
        new();

    private readonly Mock<HttpContext> _httpContextMock = new();

    private readonly Mock<ClaimsPrincipal> _userMock = new();

    public CurrentUserServiceTests()
    {
        this._httpContextMock
            .Setup(x => x.User)
            .Returns(this._userMock.Object);

        this._httpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(this._httpContextMock.Object);
    }

    [Fact]
    public void IsAuthenticated_ShouldReturnTrue_WhenUserIsAuthenticated()
    {
        this._userMock
            .Setup(x => x.Identity!.IsAuthenticated)
            .Returns(true);

        var sut = new CurrentUserService(
            this._httpContextAccessorMock.Object
        );

        sut.IsAuthenticated.ShouldBeTrue();
    }

    [Fact]
    public void UserName_ShouldReturnUserName_WhenUserIsAuthenticated()
    {
        this._userMock
            .Setup(x => x.Identity!.IsAuthenticated)
            .Returns(true);

        this._userMock
            .Setup(x => x.Identity!.Name)
            .Returns("user-1-name");

        var sut = new CurrentUserService(
            this._httpContextAccessorMock.Object
        );

        sut.UserName.ShouldBe("user-1-name");
    }

    [Fact]
    public void UserId_ShouldReturnUserId_WhenUserIsAuthenticated()
    {
        this._userMock
            .Setup(x => x.Identity!.IsAuthenticated)
            .Returns(true);

        this._userMock
            .Setup(x => x.Claims)
            .Returns(
                new List<Claim>()
                {
                    new(ClaimTypes.NameIdentifier, "user-1-id")
                }
            );

        var sut = new CurrentUserService(
            this._httpContextAccessorMock.Object
        );

        sut.UserId.ShouldBe("user-1-id");
    }

    [Fact]
    public void IsAuthenticated_ShouldReturnFalse_WhenUserIsNotAuthenticated()
    {
        this._userMock
            .Setup(x => x.Identity!.IsAuthenticated)
            .Returns(false);

        var sut = new CurrentUserService(
            this._httpContextAccessorMock.Object
        );

        sut.IsAuthenticated.ShouldBeFalse();
    }

    [Fact]
    public void UserName_ShouldReturnNull_WhenUserIsNotAuthenticated()
    {
        this._userMock
            .Setup(x => x.Identity!.IsAuthenticated)
            .Returns(false);

        var sut = new CurrentUserService(
            this._httpContextAccessorMock.Object
        );

        sut.UserName.ShouldBe(null);
    }

    [Fact]
    public void UserId_ShouldReturnNull_WhenUserIsNotAuthenticated()
    {
        this._userMock
            .Setup(x => x.Identity!.IsAuthenticated)
            .Returns(false);

        var sut = new CurrentUserService(
            this._httpContextAccessorMock.Object
        );

        sut.UserId.ShouldBe(null);
    }
}

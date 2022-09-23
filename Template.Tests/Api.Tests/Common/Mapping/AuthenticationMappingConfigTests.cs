using Mapster;
using Shouldly;
using Template.Api.Common.Mapping;
using Template.Application.Authentication.Commands.Register;
using Template.Application.Authentication.Common;
using Template.Application.Authentication.Queries.Login;
using Template.Contracts.Authentication;
using Template.Domain.Entities;
using Xunit;

namespace Template.Application.Tests.Api.Tests.Common.Mapping;

public class AuthenticationMappingConfigTests
{
    public AuthenticationMappingConfigTests()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        AuthenticationMappingConfig.AddConfig(config);
    }

    [Fact]
    public void RegisterRequest_ShouldMapTo_RegisterCommand()
    {
        var src = new RegisterRequest(
            "FirstName",
            "LastName",
            "Email",
            "Password"
        );

        var result = src.Adapt<RegisterCommand>();

        result.FirstName.ShouldBe(src.FirstName);
        result.LastName.ShouldBe(src.LastName);
        result.Email.ShouldBe(src.Email);
        result.Password.ShouldBe(src.Password);
    }

    [Fact]
    public void LoginRequest_ShouldMapTo_LoginQuery()
    {
        var src = new LoginRequest("Email", "Password");

        var result = src.Adapt<LoginQuery>();

        result.Email.ShouldBe(src.Email);
        result.Password.ShouldBe(src.Password);
    }

    [Fact]
    public void AuthenticationResult_ShouldMapTo_AuthenticationResponse_WithNullToken()
    {
        var user = new User()
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "Email",
        };

        var src = new AuthenticationResult(user);

        var result = src.Adapt<AuthenticationResponse>();

        result.FirstName.ShouldBe(user.FirstName);
        result.LastName.ShouldBe(user.LastName);
        result.Email.ShouldBe(user.Email);

        result.Token.ShouldBe(null);
    }

    [Fact]
    public void AuthenticationResult_ShouldMapTo_AuthenticationResponse_WithoutNullToken()
    {
        var user = new User()
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "Email",
        };

        var src = new AuthenticationResult(user, "token");

        var result = src.Adapt<AuthenticationResponse>();

        result.FirstName.ShouldBe(user.FirstName);
        result.LastName.ShouldBe(user.LastName);
        result.Email.ShouldBe(user.Email);

        result.Token.ShouldBe("token");
    }
}

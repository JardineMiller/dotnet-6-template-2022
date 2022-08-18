using Mapster;
using Xunit;
using Shouldly;
using Template.Api.Common.Mapping;
using Template.Application.Authentication.Queries.Login;
using Template.Contracts.Authentication;

namespace Template.Application.Tests.Api.Tests.Common.Mapping;

public class DefaultMappingConfigTests
{
    public DefaultMappingConfigTests()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        DefaultMappingConfig.AddConfig(config);
    }

    [Fact]
    public void Should_Trim_Whitespace_Properties()
    {
        var testClass = new LoginRequest(
            "email@email.com",
            "   password   "
        );

        var result = testClass.Adapt<LoginQuery>();

        result.Email.ShouldBe("email@email.com");
        result.Password.ShouldBe("password");
    }
}

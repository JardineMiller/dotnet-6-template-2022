using Microsoft.AspNetCore.Hosting;
using Moq;
using Shouldly;
using Template.Infrastructure.Email;
using Xunit;

namespace Template.Application.Tests.Infrastructure.Tests.Services;

public class EmailPathServiceTests
{
    private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock =
        new();

    private readonly IEmailPathService _emailPathService;

    public EmailPathServiceTests()
    {
        this._webHostEnvironmentMock
            .Setup(x => x.ContentRootPath)
            .Returns("Root");

        this._emailPathService = new EmailPathService(
            this._webHostEnvironmentMock.Object
        );
    }

    [Fact]
    public void EmailPathService_GetEmailPath_ReturnsCorrectPath()
    {
        var path = this._emailPathService.GetEmailPath("test.html");
        path.ShouldBe(@"Root\Email\Templates\test.html");
    }
}

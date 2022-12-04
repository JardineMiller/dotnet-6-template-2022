using System.IO;
using Moq;
using Template.Application.Common.Interfaces.Services;
using Template.Infrastructure.Email;
using Template.Infrastructure.Services;
using Xunit;

namespace Template.Application.Tests.Infrastructure.Tests.Services;

public class EmailServiceTests
{
    private readonly IEmailService _emailService;
    private readonly Mock<IEmailSender> _emailSenderMock = new();
    private readonly Mock<IEmailPathService> _emailPathServiceMock =
        new();

    public EmailServiceTests()
    {
        this._emailService = new EmailService(
            this._emailSenderMock.Object,
            this._emailPathServiceMock.Object
        );
    }

    [Fact]
    public void SendConfirmationEmail_WithValidEmail_ShouldSendEmail()
    {
        this._emailPathServiceMock
            .Setup(x => x.GetEmailPath("EmailConfirmation.html"))
            .Returns(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Email",
                    "Templates",
                    "EmailConfirmation.html"
                )
            );

        var toEmail = "to";
        var firstName = "first name";
        var token = "token";

        this._emailService.SendConfirmationEmail(
            toEmail,
            firstName,
            token
        );

        this._emailSenderMock.Verify(
            x =>
                x.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ),
            Times.Once
        );
    }

    [Fact]
    public void SendPasswordReset_WithValidEmail_ShouldSendEmail()
    {
        this._emailPathServiceMock
            .Setup(x => x.GetEmailPath("ResetPassword.html"))
            .Returns(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Email",
                    "Templates",
                    "ResetPassword.html"
                )
            );

        var toEmail = "to";
        var token = "token";

        this._emailService.SendPasswordResetEmail(toEmail, token);

        this._emailSenderMock.Verify(
            x =>
                x.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ),
            Times.Once
        );
    }
}

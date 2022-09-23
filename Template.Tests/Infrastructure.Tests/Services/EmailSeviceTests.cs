using Moq;
using Template.Application.Common.Interfaces.Services;
using Template.Infrastructure.Services;
using Xunit;

namespace Template.Application.Tests.Infrastructure.Tests.Services;

public class EmailSeviceTests
{
    private readonly IEmailService _emailService;
    private readonly Mock<IEmailSender> _emailSenderMock;

    public EmailSeviceTests()
    {
        this._emailSenderMock = new Mock<IEmailSender>();
        this._emailService = new EmailService(
            this._emailSenderMock.Object
        );
    }

    //TODO: Not sure how to handle the file path to template...
    // [Fact]
    // public void SendEmailAsync_WithValidEmail_ShouldSendEmail()
    // {
    //     var toEmail = "to";
    //     var firstName = "first name";
    //     var token = "token";
    //
    //     this._emailService.SendConfirmationEmail(
    //         toEmail,
    //         firstName,
    //         token
    //     );
    //     this._emailSenderMock.Verify(
    //         x =>
    //             x.SendEmail(
    //                 It.IsAny<string>(),
    //                 It.IsAny<string>(),
    //                 It.IsAny<string>()
    //             ),
    //         Times.Once
    //     );
    // }
}

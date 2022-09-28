using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Template.Infrastructure.Email;
using Template.Infrastructure.Services;
using Xunit;

namespace Template.Application.Tests.Infrastructure.Tests.Services;

public class EmailSenderTests
{
    private EmailSender _emailSender = null!;

    private readonly Mock<
        IOptions<EmailSettings>
    > _emailSettingsMock = new();

    [Fact]
    public void SendEmailAsync_WithValidEmail_ShouldSendEmail()
    {
        // Arrange
        var emailSettings = new EmailSettings
        {
            From = "testing@fileshare.com",
            Host = "smtp.mailtrap.io",
            Port = 2525,
            DisplayName = "File Share - Development",
            Username = "790ae629ae2ce0",
            Password = "348bf350b3ee06"
        };

        this._emailSettingsMock
            .Setup(x => x.Value)
            .Returns(emailSettings);

        this._emailSender = new EmailSender(
            this._emailSettingsMock.Object
        );

        var result = this._emailSender.SendEmail(
            "test@test.com",
            "Test",
            "Test"
        );

        result.ShouldBe(true);
    }

    [Fact]
    public void SendEmailAsync_WithInvalidEmail_ShouldNotSendEmail()
    {
        // Arrange
        var emailSettings = new EmailSettings
        {
            From = "testing@fileshare.com",
            Host = "smtp.mailtrap.io",
            Port = 2525,
            DisplayName = "File Share - Development",
            Username = "",
            Password = ""
        };

        this._emailSettingsMock
            .Setup(x => x.Value)
            .Returns(emailSettings);

        this._emailSender = new EmailSender(
            this._emailSettingsMock.Object
        );

        var result = this._emailSender.SendEmail(
            "test@test.com",
            "Test",
            "Test"
        );

        result.ShouldBe(false);
    }
}

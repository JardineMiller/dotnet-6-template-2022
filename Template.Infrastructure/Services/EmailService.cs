using System.Web;
using Template.Application.Common.Interfaces.Services;

namespace Template.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IEmailSender _emailSender;

    public EmailService(IEmailSender emailSender)
    {
        this._emailSender = emailSender;
    }

    public void SendConfirmationEmail(
        string toEmail,
        string firstName,
        string token
    )
    {
        var filePath = Directory.GetCurrentDirectory();
        var parent = Directory.GetParent(filePath);

        var streamReader = new StreamReader(
            $"{parent}/Template.Infrastructure/Email/Templates/EmailConfirmation.html"
        );
        var mailText = streamReader.ReadToEnd();
        streamReader.Close();

        var encodedToken = HttpUtility.UrlEncode(token);

        mailText = mailText
            .Replace("[username]", firstName)
            .Replace("[email]", toEmail)
            .Replace(
                "[welcome-link]",
                $"https://localhost:7097/api/account/confirm?token={encodedToken}&email={toEmail}"
            );

        this._emailSender.SendEmail(
            toEmail,
            $"Welcome to File Share, {firstName}",
            mailText
        );
    }
}

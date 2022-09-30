using System.Web;
using Template.Application.Common.Interfaces.Services;
using Template.Infrastructure.Email;

namespace Template.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly IEmailPathService _emailPathService;

    public EmailService(
        IEmailSender emailSender,
        IEmailPathService emailPathService
    )
    {
        this._emailSender = emailSender;
        this._emailPathService = emailPathService;
    }

    public void SendConfirmationEmail(
        string toEmail,
        string firstName,
        string token
    )
    {
        var mailText = GetEmailContent("EmailConfirmation.html");
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

    public void SendPasswordResetEmail(string toEmail, string token)
    {
        var mailText = GetEmailContent("ResetPassword.html");
        var encodedToken = HttpUtility.UrlEncode(token);

        mailText = mailText
            .Replace("[email]", toEmail)
            .Replace(
                "[reset-link]",
                $"https://localhost:7097/api/account/resetpassword?token={encodedToken}&email={toEmail}"
            );

        this._emailSender.SendEmail(
            toEmail,
            "File Share - Reset Password",
            mailText
        );
    }

    private string GetEmailContent(string emailFileName)
    {
        var streamReader = new StreamReader(
            this._emailPathService.GetEmailPath(emailFileName)
        );

        var mailText = streamReader.ReadToEnd();
        streamReader.Close();

        return mailText;
    }
}

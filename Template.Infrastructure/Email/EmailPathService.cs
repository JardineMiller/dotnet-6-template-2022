using Microsoft.AspNetCore.Hosting;

namespace Template.Infrastructure.Email;

public class EmailPathService : IEmailPathService
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public EmailPathService(IWebHostEnvironment hostingEnvironment)
    {
        this._hostingEnvironment = hostingEnvironment;
    }

    public string GetEmailPath(string emailName)
    {
        return Path.Combine(
            this._hostingEnvironment.ContentRootPath,
            "Email",
            "Templates",
            emailName
        );
    }
}

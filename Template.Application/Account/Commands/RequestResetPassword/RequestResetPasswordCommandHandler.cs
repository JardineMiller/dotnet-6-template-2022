using System.Web;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Template.Application.Account.Common;
using Template.Application.Common.Interfaces.Services;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;

namespace Template.Application.Account.Commands.RequestResetPassword;

public class RequestResetPasswordCommandHandler
    : IRequestHandler<
          RequestResetPasswordCommand,
          ErrorOr<RequestResetPasswordResult>
      >
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public RequestResetPasswordCommandHandler(
        UserManager<User> userManager,
        IEmailService emailService
    )
    {
        this._userManager = userManager;
        this._emailService = emailService;
    }

    public async Task<ErrorOr<RequestResetPasswordResult>> Handle(
        RequestResetPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        if (
            await this._userManager.FindByEmailAsync(request.Email)
            is not User user
        )
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token =
            await this._userManager.GeneratePasswordResetTokenAsync(
                user
            );

        var encodedToken = HttpUtility.UrlEncode(token);

        this._emailService.SendPasswordResetEmail(user.Email, token);

        return new RequestResetPasswordResult(encodedToken);
    }
}

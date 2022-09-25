using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Template.Application.Account.Common;
using Template.Domain.Common.Errors;
using Template.Domain.Entities;

namespace Template.Application.Account.Commands.ResetPassword;

public class ResetPasswordCommandHandler
    : IRequestHandler<
          ResetPasswordCommand,
          ErrorOr<ResetPasswordResult>
      >
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        this._userManager = userManager;
    }

    public async Task<ErrorOr<ResetPasswordResult>> Handle(
        ResetPasswordCommand cmd,
        CancellationToken cancellationToken
    )
    {
        return cmd.Token is null
          ? await ChangePassword(cmd)
          : await ResetPassword(cmd);
    }

    private async Task<ErrorOr<ResetPasswordResult>> ChangePassword(
        ResetPasswordCommand cmd
    )
    {
        if (
            await this._userManager.FindByEmailAsync(cmd.Email)
            is not User user
        )
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var changeResult =
            await this._userManager.ChangePasswordAsync(
                user,
                cmd.OldPassword,
                cmd.NewPassword
            );

        if (changeResult != IdentityResult.Success)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        return new ResetPasswordResult();
    }

    private async Task<ErrorOr<ResetPasswordResult>> ResetPassword(
        ResetPasswordCommand cmd
    )
    {
        if (
            await this._userManager.FindByEmailAsync(cmd.Email)
            is not User user
        )
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var changeResult = await this._userManager.ResetPasswordAsync(
            user,
            cmd.Token,
            cmd.NewPassword
        );

        if (changeResult != IdentityResult.Success)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        return new ResetPasswordResult();
    }
}

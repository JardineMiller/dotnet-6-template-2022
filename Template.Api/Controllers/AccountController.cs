using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Account.Commands.RequestResetPassword;
using Template.Application.Account.Commands.ResetPassword;
using Template.Contracts.Account.RequestResetPassword;
using Template.Contracts.Account.ResetPassword;

namespace Template.Api.Controllers;

public class AccountController : ApiController
{
    private readonly ISender _mediator;

    public AccountController(ISender mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet(nameof(ResetPassword))]
    public async Task<IActionResult> ResetPassword(
        ResetPasswordRequest request
    )
    {
        var cmd = request.Adapt<ResetPasswordCommand>();
        var result = await this._mediator.Send(cmd);

        return result.Match(
            success => Ok(success.Adapt<ResetPasswordResponse>()),
            errors => Problem(errors)
        );
    }

    [AllowAnonymous]
    [HttpGet(nameof(RequestResetPassword))]
    public async Task<IActionResult> RequestResetPassword(
        RequestResetPasswordRequest request
    )
    {
        var cmd = request.Adapt<RequestResetPasswordCommand>();
        var result = await this._mediator.Send(cmd);

        return result.Match(
            success =>
                Ok(success.Adapt<RequestResetPasswordResponse>()),
            errors => Problem(errors)
        );
    }
}

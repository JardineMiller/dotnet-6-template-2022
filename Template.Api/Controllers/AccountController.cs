using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Account.Commands;
using Template.Contracts.Account;

namespace Template.Api.Controllers;

public class AccountController : ApiController
{
    private readonly ISender _mediator;

    public AccountController(ISender mediator)
    {
        this._mediator = mediator;
    }

    [HttpPost(nameof(ResetPassword))]
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
}

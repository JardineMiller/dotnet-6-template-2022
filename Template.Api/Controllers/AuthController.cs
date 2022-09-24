using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Authentication.Commands.ConfirmEmail;
using Template.Application.Authentication.Commands.Register;
using Template.Application.Authentication.Queries.Login;
using Template.Contracts.Authentication;

namespace Template.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        this._mediator = mediator;
    }

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var cmd = request.Adapt<RegisterCommand>();
        var authResult = await this._mediator.Send(cmd);

        return authResult.Match(
            result => Ok(result.Adapt<AuthenticationResponse>()),
            errors => Problem(errors)
        );
    }

    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var qry = request.Adapt<LoginQuery>();
        var authResult = await this._mediator.Send(qry);

        return authResult.Match(
            result => Ok(result.Adapt<AuthenticationResponse>()),
            errors => Problem(errors)
        );
    }

    [HttpGet(nameof(Confirm))]
    public async Task<IActionResult> Confirm(
        string token,
        string email
    )
    {
        var cmd = new ConfirmEmailCommand(email, token);
        var authResult = await this._mediator.Send(cmd);

        return authResult.Match(
            result => Ok(result.Adapt<AuthenticationResponse>()),
            errors => Problem(errors)
        );
    }
}

using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Authentication.Commands.Register;
using Template.Application.Authentication.Queries.Login;
using Template.Contracts.Authentication;

namespace Template.Api.Controllers;

public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;

    public AuthenticationController(ISender mediator)
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
}

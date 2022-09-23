using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers;

[AllowAnonymous]
public class ErrorsController : ApiController
{
    [Route(nameof(Error))]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features
            .Get<IExceptionHandlerFeature>()
            ?.Error;

        var (statusCode, message) = exception switch
        {
            _
              => (
                  StatusCodes.Status500InternalServerError,
                  "An unexpected error occured"
              )
        };

        return Problem(statusCode: statusCode, title: message);
    }
}

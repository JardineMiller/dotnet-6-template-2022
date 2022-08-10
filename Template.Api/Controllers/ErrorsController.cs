using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Common.Errors;

namespace Template.Api.Controllers;

public class ErrorsController : ApiController
{
    [Route("/error")]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features
            .Get<IExceptionHandlerFeature>()
            ?.Error;

        var (statusCode, message) = exception switch
        {
            DuplicateEmailException
              => (
                  StatusCodes.Status409Conflict,
                  "Email already exists"
              ),
            _
              => (
                  StatusCodes.Status500InternalServerError,
                  "An unexpected error occured"
              )
        };

        return Problem(statusCode: statusCode, title: message);
    }
}
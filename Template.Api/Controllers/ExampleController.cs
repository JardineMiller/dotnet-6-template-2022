using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers;

public class ExampleController : ApiController
{
    [HttpGet]
    public IActionResult Example()
    {
        return Ok(Array.Empty<string>());
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace miniTodo.Api.Controllers.Test;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("Public")]
    public IActionResult Public()
    {
        return Ok("Hello stranger");
    }

    [HttpGet("Secure")]
    [Authorize]
    public IActionResult Secure()
    {
        return Ok("Hello user");
    }
}

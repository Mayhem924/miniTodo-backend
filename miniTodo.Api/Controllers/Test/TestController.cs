using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace miniTodo.Api.Controllers.Test;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    /// <summary>
    /// Doesn't require a JWT token
    /// </summary>
    /// <returns></returns>
    [HttpGet("Public")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Public()
    {
        return Ok("Hello stranger");
    }

    /// <summary>
    /// Requires a JWT token
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("Secure")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Secure()
    {
        return Ok("Hello user");
    }
}

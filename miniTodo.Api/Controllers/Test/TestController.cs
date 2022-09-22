namespace miniTodo.Api.Controllers.Test;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class TestController : ControllerBase
{
    /// <summary>
    /// Doesn't require a JWT token
    /// </summary>
    [HttpGet("test/public")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Public()
    {
        return Ok("Hello stranger");
    }

    /// <summary>
    /// Requires a JWT token
    /// </summary>
    [Authorize]
    [HttpGet("test/secure")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Secure()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        var userName = User.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier"))?.Value;

        return Ok(new { userId, userName });
    }
}

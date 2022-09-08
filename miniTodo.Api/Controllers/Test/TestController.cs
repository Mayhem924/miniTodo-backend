using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniTodo.Api.Services.JwtGenerator;
using System.IdentityModel.Tokens.Jwt;

namespace miniTodo.Api.Controllers.Test;

[ApiController]
public class TestController : ControllerBase
{
    private readonly IJwtGenerator jwtGenerator;

    public TestController(IJwtGenerator jwtGenerator)
    {
        this.jwtGenerator = jwtGenerator;
    }

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
        
        var userInfo = new
        {
            userId = userId,
            userName = userName
        };

        return Ok(userInfo);
    }
}

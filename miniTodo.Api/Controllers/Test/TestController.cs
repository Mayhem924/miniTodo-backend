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
    /// <returns></returns>
    [HttpGet("test/public")]
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
    [HttpGet("test/secure")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Secure()
    {
        var token = ControllerContext.HttpContext.Request.Headers
            .Single(x => x.Key == "Authorization").Value[0]
            .Split(' ')[1];

        if (string.IsNullOrEmpty(token))
            return BadRequest();

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var userInfo = new
        {
            userId = jwtToken.Claims.First(x => x.Type == "id").Value,
            userName = jwtToken.Subject
        };

        return Ok(userInfo);
    }
}

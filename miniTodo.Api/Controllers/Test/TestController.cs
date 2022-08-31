using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace miniTodo.Api.Controllers.Test;

[ApiController]
public class TestController : ControllerBase
{
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
    public IActionResult Secure()
    {
        var token = ControllerContext.HttpContext.Request.Headers
            .Single(x => x.Key == "Authorization").Value[0]
            .Split(' ')[1];

        if (string.IsNullOrEmpty(token))
            return BadRequest();

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValue = tokenHandler.ReadJwtToken(token);

        var userInfo = new
        {
            userId = tokenValue?.Claims.Single(x => x.Type == "id").Value,
            userName = tokenValue?.Subject
        };

        return Ok(userInfo);
    }
}

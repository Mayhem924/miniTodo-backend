namespace miniTodo.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using miniTodo.Api.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings jwtSettings;

    public AuthController(JwtSettings jwtSettings)
	{
        this.jwtSettings = jwtSettings;
	}

    [HttpGet]
    public IActionResult Authenticate()
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "Nick"),
            new Claim(JwtRegisteredClaimNames.Email, "test@email.com"),
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            Constants.Issuer,
            Constants.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            signingCredentials);

        var value = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(value);
    }
}

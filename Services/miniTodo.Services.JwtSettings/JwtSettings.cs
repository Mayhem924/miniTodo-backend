using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace miniTodo.Services.JwtSettings;

public class JwtSettings : IJwtSettings
{
	public string Secret { get; init; }

	public JwtSettings()
	{
		//Secret = configuration["JwtSettings:Secret"];
		Secret = "O9KLvFcjRj8oaRzVggpCEbQd3Py4qOL8k0gIoZhDqa"; // TODO: Replace with IConfiguration
	}

	public string GenerateToken(string userName)
	{
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            null,
            null,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            signingCredentials);

        var value = new JwtSecurityTokenHandler().WriteToken(token);
        return value;
    }
}

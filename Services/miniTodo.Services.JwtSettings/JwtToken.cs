using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace miniTodo.Services.JwtToken;

public class JwtToken : IJwtToken
{
    public string Secret { get; init; }

    public string GenerateToken(string userName, string userEmail)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userEmail),
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

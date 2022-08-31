namespace miniTodo.Services.JwtToken;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using miniTodo.Data;
using miniTodo.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtGenerator : IJwtGenerator
{
    public string Secret { get; init; }
    public TimeSpan AccessTokenLifetime { get; init; }

    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public JwtGenerator(IDbContextFactory<ApplicationDbContext> contextFactory, IConfiguration configuration)
    {
        this.contextFactory = contextFactory;

        Secret = configuration["JwtSettings:Secret"];
        AccessTokenLifetime = TimeSpan.Parse(configuration["JwtSettings:AccessTokenLifetime"]);
    }

    public async Task<string> GenerateToken(User user)
    {
        using var dbContext = await contextFactory.CreateDbContextAsync();

        var key = Encoding.ASCII.GetBytes(Secret);

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = credentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

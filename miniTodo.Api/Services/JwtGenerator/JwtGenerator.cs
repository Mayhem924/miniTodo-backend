namespace miniTodo.Api.Services.JwtGenerator;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using miniTodo.Api.Data;
using miniTodo.Api.Data.Entities;
using miniTodo.Api.Services.JwtGenerator.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JwtGenerator : IJwtGenerator
{
    public string Secret { get; init; }
    public TimeSpan AccessTokenLifetime { get; init; }

    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;
    private readonly TokenValidationParameters validationParameters;

    public JwtGenerator(
        IDbContextFactory<ApplicationDbContext> contextFactory,
        IConfiguration configuration,
        TokenValidationParameters validationParameters)
    {
        this.contextFactory = contextFactory;
        this.validationParameters = validationParameters;

        Secret = configuration["JwtSettings:Secret"];
        AccessTokenLifetime = TimeSpan.Parse(configuration["JwtSettings:AccessTokenLifetime"]);
    }

    public async Task<string> GenerateAccessToken(User user)
    {
        using var dbContext = await contextFactory.CreateDbContextAsync();

        var key = Encoding.ASCII.GetBytes(Secret);

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = credentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<RefreshTokenModel> RefreshToken(RefreshTokenModel model)
    {
        using var dbContext = await contextFactory.CreateDbContextAsync();
        var userId = GetUserIdFromAccessToken(model.AccessToken);

        var user = dbContext.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefault(x => x.Id == userId);

        if (user is null)
            throw new Exception("User not found!");

        var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);

        if (user.Id != refreshToken?.UserId)
            throw new Exception("Refresh token doesn't match with JWT user");

        if (refreshToken.ExpiryDate <= DateTime.UtcNow)
        {
            dbContext.RefreshTokens.Remove(refreshToken);
            await dbContext.SaveChangesAsync();

            throw new Exception("Refresh token is expired!");
        }

        dbContext.RefreshTokens.Remove(refreshToken);

        var token = GenerateRefreshToken();
        token.User = user;
        token.UserId = user.Id;

        await dbContext.RefreshTokens.AddAsync(token);
        await dbContext.SaveChangesAsync();

        var result = new RefreshTokenModel
        {
            AccessToken = await GenerateAccessToken(user),
            RefreshToken = token.Token
        };

        return result;
    }

    public RefreshToken GenerateRefreshToken()
    {
        var bytes = new byte[32];

        using var random = RandomNumberGenerator.Create();
        random.GetBytes(bytes);

        var token = new RefreshToken
        {
            Token = Convert.ToBase64String(bytes),
            ExpiryDate = DateTime.UtcNow.AddMonths(6)
        };

        return token;
    }

    private Guid? GetUserIdFromAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Secret);

        var principle = tokenHandler.ValidateToken(accessToken, validationParameters, out var securityToken);
        var jwtToken = securityToken as JwtSecurityToken;

        if (jwtToken is null)
            return null;

        if (!jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            return null;

        var userIdClaim = principle.Claims.First(x => x.Type == "id");
        var userId = Guid.Parse(userIdClaim.Value);

        return userId;
    }
}

namespace miniTodo.Api.Services.JwtGenerator;

using miniTodo.Api.Data.Entities;
using miniTodo.Api.Services.JwtGenerator.Models;

public interface IJwtGenerator
{
    public string Secret { get; }
    public TimeSpan AccessTokenLifetime { get; }

    Task<string> GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken();
    Task<RefreshTokenModel> RefreshToken(RefreshTokenModel model);
}

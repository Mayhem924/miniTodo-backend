namespace miniTodo.Services.JwtToken;

using miniTodo.Api.Data.Entities;

public interface IJwtGenerator
{
    public string Secret { get; }
    public TimeSpan AccessTokenLifetime { get; }

    Task<string> GenerateToken(User user);
}

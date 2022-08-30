using miniTodo.Data.Entities;

namespace miniTodo.Services.JwtToken;

public interface IJwtGenerator
{
    public string Secret { get; }
    public TimeSpan AccessTokenLifetime { get; }

    Task<string> GenerateToken(User user);
}

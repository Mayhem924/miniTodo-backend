namespace miniTodo.Services.JwtSettings;

public interface IJwtSettings
{
    string GenerateToken(string userName);
}

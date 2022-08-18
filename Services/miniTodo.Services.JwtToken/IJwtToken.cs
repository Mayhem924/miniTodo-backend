namespace miniTodo.Services.JwtToken;

public interface IJwtToken
{
    string GenerateToken(string userName, string userEmail);
}

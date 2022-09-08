namespace miniTodo.Api.Services.JwtGenerator;

using miniTodo.Api.Data.Entities;
using miniTodo.Api.Services.JwtGenerator.Models;

public interface IJwtGenerator
{
    Task<string> GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken();
    Task<RefreshTokenModel> RefreshToken(RefreshTokenModel model);
}

namespace miniTodo.Api.Controllers.Token.Models;

public class RefreshTokenRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

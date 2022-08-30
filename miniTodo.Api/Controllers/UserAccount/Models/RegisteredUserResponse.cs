namespace miniTodo.Api.Controllers.UserAccount.Models;

public class AuthenticationResponse
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

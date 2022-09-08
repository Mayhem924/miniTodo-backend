using System.ComponentModel;

namespace miniTodo.Api.Controllers.UserAccount.Models;

public class RegisterUserRequest
{
    [DefaultValue("TestUser")]
    public string UserName { get; set; }

    [DefaultValue("test@email.com")]
    public string Email { get; set; }

    [DefaultValue("Pass123")]
    public string Password { get; set; }

    [DefaultValue("Pass123")]
    public string ConfirmPassword { get; set; }
}

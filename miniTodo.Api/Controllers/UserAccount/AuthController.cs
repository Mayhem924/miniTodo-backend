namespace miniTodo.Api.Controllers.UserAccount;

using Microsoft.AspNetCore.Mvc;
using miniTodo.Api.Controllers.UserAccount.Models;
using miniTodo.Services.UserAccount;
using miniTodo.Services.UserAccount.Models;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserAccount userAccount;

    public AuthController(IUserAccount userAccount)
    {
        this.userAccount = userAccount;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var model = new LoginUserModel
        {
            UserName = request.UserName,
            Password = request.Password
        };

        var token = await userAccount.Login(model);
        return Ok(token);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            throw new Exception("Password confirmation is failed");

        var model = new RegisterUserModel
        {
            UserName = request.UserName,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword
        };

        var token = await userAccount.Register(model);
        return Ok(token);
    }
}

namespace miniTodo.Api.Controllers.UserAccount;

using Microsoft.AspNetCore.Mvc;
using miniTodo.Api.Controllers.UserAccount.Models;
using miniTodo.Services.JwtToken;
using miniTodo.Services.UserAccount;
using miniTodo.Services.UserAccount.Models;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserAccount userAccount;
    private readonly IJwtGenerator jwtGenerator;

    public AuthController(IUserAccount userAccount, IJwtGenerator jwtGenerator)
    {
        this.userAccount = userAccount;
        this.jwtGenerator = jwtGenerator;
    }

    /// <summary>
    /// Get token with a registered user information
    /// </summary>
    /// <param name="request">User's data</param>
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var model = new LoginUserModel
        {
            UserName = request.UserName,
            Password = request.Password
        };

        var user = await userAccount.Login(model);
        var token = await jwtGenerator.GenerateToken(user);

        var result = new AuthenticationResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            AccessToken = token,
            RefreshToken = null
        };

        return Ok(result);
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">User's data</param>
    /// <returns>Authentication result</returns>
    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthenticationResponse))]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            throw new Exception("Password confirmation is failed");

        var model = new RegisterUserModel
        {
            UserName = request.UserName,
            Password = request.Password
        };

        var user = await userAccount.Register(model);
        var token = await jwtGenerator.GenerateToken(user);

        var result = new AuthenticationResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            AccessToken = token,
            RefreshToken = null
        };

        return Created(string.Empty, result);
    }
}

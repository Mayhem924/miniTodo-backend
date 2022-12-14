namespace miniTodo.Api.Controllers.UserAccount;

using Microsoft.AspNetCore.Mvc;
using Models;
using Services.JwtGenerator;
using miniTodo.Api.Services.UserAccount;
using miniTodo.Api.Services.UserAccount.Models;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserAccount userAccount;
    private readonly IJwtGenerator jwtGenerator;

    public UserController(IUserAccount userAccount, IJwtGenerator jwtGenerator)
    {
        this.userAccount = userAccount;
        this.jwtGenerator = jwtGenerator;
    }

    /// <summary>
    /// Get token with a registered user information
    /// </summary>
    /// <param name="request">User's data</param>
    [HttpPost("user/login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var model = new LoginUserModel
        {
            UserName = request.UserName,
            Password = request.Password
        };

        var refreshToken = await userAccount.Login(model);
        var token = await jwtGenerator.GenerateAccessToken(refreshToken.User);

        var result = new AuthenticationResponse
        {
            UserId = refreshToken.User.Id,
            UserName = refreshToken.User.UserName,
            AccessToken = token,
            RefreshToken = refreshToken.Token
        };

        return Ok(result);
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">User's data</param>
    /// <returns>Authentication result</returns>
    [HttpPost("user/register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthenticationResponse))]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            throw new Exception("Password confirmation is failed");

        var model = new RegisterUserModel
        {
            UserName = request.UserName,
            Email = request.Email,
            Password = request.Password
        };

        var user = await userAccount.Register(model);
        var token = await jwtGenerator.GenerateAccessToken(user);

        var result = new AuthenticationResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            AccessToken = token,
            RefreshToken = user.RefreshTokens.First().Token
        };

        return Ok(result);
    }
}

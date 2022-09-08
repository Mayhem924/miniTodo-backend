namespace miniTodo.Api.Controllers.Token;

using Microsoft.AspNetCore.Mvc;
using miniTodo.Api.Controllers.Token.Models;
using miniTodo.Api.Services.JwtGenerator;
using miniTodo.Api.Services.JwtGenerator.Models;

[ApiController]
public class TokenController : ControllerBase
{
    private readonly IJwtGenerator jwtGenerator;

	public TokenController(IJwtGenerator jwtGenerator)
	{
		this.jwtGenerator = jwtGenerator;
	}

	/// <summary>
	/// Get a new access token with a refresh token
	/// </summary>
	/// <param name="request">Tokens</param>
	[HttpPost("token/refresh")]
	public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
	{
		var model = new RefreshTokenModel
		{
			AccessToken = request.AccessToken,
			RefreshToken = request.RefreshToken
		};

		var result = await jwtGenerator.RefreshToken(model);

		return Ok(result);
	}
}

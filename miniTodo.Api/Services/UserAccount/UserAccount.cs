namespace miniTodo.Api.Services.UserAccount;

using Microsoft.EntityFrameworkCore;
using miniTodo.Api.Data;
using miniTodo.Api.Data.Entities;
using miniTodo.Api.Services.JwtGenerator;
using miniTodo.Api.Services.UserAccount.Models;
using System.Security.Cryptography;
using System.Text;

public class UserAccount : IUserAccount
{
	private readonly IDbContextFactory<ApplicationDbContext> contextFactory;
	private readonly IJwtGenerator jwtGenerator;

	public UserAccount(IDbContextFactory<ApplicationDbContext> contextFactory, IJwtGenerator jwtGenerator)
	{
		this.contextFactory = contextFactory;
		this.jwtGenerator = jwtGenerator;
	}

	public async Task<RefreshToken> Login(LoginUserModel model)
	{
		using var dbContext = await contextFactory.CreateDbContextAsync();

		var user = await dbContext.Users
			.Include(x => x.RefreshTokens)
			.FirstOrDefaultAsync(x => x.UserName == model.UserName);

		if (user is null)
			throw new Exception("User not found!");

		var hashedPassword = HashPasswordSHA256(model.Password);

		if (user.PasswordHash != hashedPassword)
			throw new Exception("Incorrect password!");

		var refreshToken = jwtGenerator.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
		await dbContext.SaveChangesAsync();

		return refreshToken;
	}

	public async Task<User> Register(RegisterUserModel model)
	{
		using var dbContext = await contextFactory.CreateDbContextAsync();

		if (await dbContext.Users.AnyAsync(x => x.UserName == model.UserName || x.Email == model.Email))
			throw new Exception("This user credentionals are already taken!");

		var user = new User
		{
			UserName = model.UserName,
			Email = model.Email,
			PasswordHash = HashPasswordSHA256(model.Password),
			RefreshTokens = new List<RefreshToken> { jwtGenerator.GenerateRefreshToken() }
		};

		await dbContext.Users.AddAsync(user);
		await dbContext.SaveChangesAsync();

		return user;
	}

	private static string HashPasswordSHA256(string password)
	{
		var valueBytes = Encoding.UTF8.GetBytes(password);
		var hashedValue = SHA256.HashData(valueBytes);

		return Convert.ToHexString(hashedValue);
	}
}

namespace miniTodo.Services.UserAccount;

using Microsoft.EntityFrameworkCore;
using miniTodo.Data;
using miniTodo.Data.Entities;
using miniTodo.Services.UserAccount.Models;
using System.Security.Cryptography;
using System.Text;

public class UserAccount : IUserAccount
{
	private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

	public UserAccount(IDbContextFactory<ApplicationDbContext> contextFactory)
	{
		this.contextFactory = contextFactory;
	}

	public async Task<User> Login(LoginUserModel model)
	{
		using var dbContext = await contextFactory.CreateDbContextAsync();

		var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);

		if (user is null)
			throw new Exception("User not found!");

		var hashedPassword = HashPasswordSHA256(model.Password);

		if (user.PasswordHash != hashedPassword)
			throw new Exception("Incorrect password!");

		return user;
	}

	public async Task<User> Register(RegisterUserModel model)
	{
		using var dbContext = await contextFactory.CreateDbContextAsync();

		if (dbContext.Users.Any(x => x.UserName == model.UserName))
			throw new Exception("This UserName is already taken!");

        if (dbContext.Users.Any(x => x.Email == model.Email))
            throw new Exception("This Email is already taken!");

        var user = new User
		{
			UserName = model.UserName,
			Email = model.Email,
			PasswordHash = HashPasswordSHA256(model.Password),
		};

		dbContext.Users.Add(user);
		await dbContext.SaveChangesAsync();

		return user;
	}

	public async Task<string> RefreshToken(RefreshTokenModel model)
	{
		return null;
	}

	private static string HashPasswordSHA256(string password)
	{
		var valueBytes = Encoding.UTF8.GetBytes(password);
		var hashedValue = SHA256.HashData(valueBytes);

		return Convert.ToHexString(hashedValue);
	}
}

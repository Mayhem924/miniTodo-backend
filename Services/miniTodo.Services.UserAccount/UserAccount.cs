namespace miniTodo.Services.UserAccount;

using Microsoft.EntityFrameworkCore;
using miniTodo.Data;
using miniTodo.Data.Entities;
using miniTodo.Services.JwtToken;
using miniTodo.Services.UserAccount.Models;

public class UserAccount : IUserAccount
{
	private readonly IDbContextFactory<ApplicationDbContext> contextFactory;
	private readonly IJwtToken jwtSettings;

	public UserAccount(
		IDbContextFactory<ApplicationDbContext> contextFactory,
		IJwtToken jwtSettings)
	{
		this.contextFactory = contextFactory;
		this.jwtSettings = jwtSettings;
	}

	public async Task<string> Login(LoginUserModel model)
	{
		using var dbContext = await contextFactory.CreateDbContextAsync();
		var user = dbContext.Users.FirstOrDefault(x => x.Name == model.UserName && x.Password == model.Password);

		if (user is null)
			throw new Exception("Incorrect UserName or Password");

		var token = jwtSettings.GenerateToken(user.Name);
		return token;
	}

	public async Task<string> Register(RegisterUserModel model)
	{
		using var dbContext = await contextFactory.CreateDbContextAsync();

		if (dbContext.Users.Any(x => x.Name == model.UserName))
			throw new Exception("This UserName is already taken!");

        if (dbContext.Users.Any(x => x.Email == model.Email))
            throw new Exception("This Email is already taken!");

        var user = new User
		{
			Name = model.UserName,
			Password = model.Password,
		};

		dbContext.Users.Add(user);
		await dbContext.SaveChangesAsync();

		var token = jwtSettings.GenerateToken(user.Name);
		return token;
	}
}

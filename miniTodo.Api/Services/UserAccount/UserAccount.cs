namespace miniTodo.Api.Services.UserAccount;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Entities;
using JwtGenerator;
using Models;

public class UserAccount : IUserAccount
{
	private readonly IDbContextFactory<ApplicationDbContext> contextFactory;
	private readonly IPasswordHasher<User> passwordHasher;
	private readonly IJwtGenerator jwtGenerator;

	public UserAccount(
		IDbContextFactory<ApplicationDbContext> contextFactory,
		IPasswordHasher<User> passwordHasher,
		IJwtGenerator jwtGenerator)
	{
		this.contextFactory = contextFactory;
		this.passwordHasher = passwordHasher;
		this.jwtGenerator = jwtGenerator;
	}

	public async Task<RefreshToken> Login(LoginUserModel model)
	{
		await using var dbContext = await contextFactory.CreateDbContextAsync();

		var user = await dbContext.Users
			.Include(x => x.RefreshTokens)
			.FirstOrDefaultAsync(x => x.UserName == model.UserName);

		if (user is null)
			throw new Exception("User not found!");
		
		var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

		if (result == PasswordVerificationResult.Failed)
			throw new Exception("Incorrect password!");

		var refreshToken = jwtGenerator.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
		await dbContext.SaveChangesAsync();

		return refreshToken;
	}

	public async Task<User> Register(RegisterUserModel model)
	{
		await using var dbContext = await contextFactory.CreateDbContextAsync();

		if (await dbContext.Users.AnyAsync(x => x.UserName == model.UserName || x.Email == model.Email))
			throw new Exception("This user credentionals are already taken!");

		var user = new User
		{
			UserName = model.UserName,
			Email = model.Email,
			RefreshTokens = new List<RefreshToken> { jwtGenerator.GenerateRefreshToken() }
		};

		user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

		await dbContext.Users.AddAsync(user);
		await dbContext.SaveChangesAsync();

		return user;
	}
}

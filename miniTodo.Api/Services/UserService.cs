namespace miniTodo.Api.Services;

using Microsoft.EntityFrameworkCore;
using miniTodo.Api.Options;
using miniTodo.Data;

public class UserService
{
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

	public UserService(IDbContextFactory<ApplicationDbContext> contextFactory)
	{
		this.contextFactory = contextFactory;
	}
}

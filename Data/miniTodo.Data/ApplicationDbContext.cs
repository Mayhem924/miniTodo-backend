namespace miniTodo.Data;

using Microsoft.EntityFrameworkCore;
using miniTodo.Data.Entities;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        //Database.EnsureCreated();
    }
}

using Microsoft.EntityFrameworkCore;
using miniTodo.Api.Configuration;
using miniTodo.Data;
using miniTodo.Services.JwtToken;
using miniTodo.Services.UserAccount;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Services
services.AddControllers();

services.AddEndpointsApiExplorer();

// Configurations
services.AddAppAuthentication(builder.Configuration);
services.AddAppSwagger();

services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlite(connectionString);
});

services.AddSingleton<IUserAccount, UserAccount>();
services.AddSingleton<IJwtGenerator, JwtGenerator>();

// Build application
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAppSwagger(builder.Configuration);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    var dbContext = contextFactory.CreateDbContext();

    dbContext.Database.Migrate();
}

app.Run();

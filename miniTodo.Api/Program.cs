using Microsoft.EntityFrameworkCore;
using miniTodo.Api.Configuration;
using miniTodo.Data;
using miniTodo.Services.UserAccount;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Services
services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// Configurations
services.AddAppAuthentication(builder.Configuration);

services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlite(connectionString);
});

services.AddSingleton<IUserAccount, UserAccount>();

// Build application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

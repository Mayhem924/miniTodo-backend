using Microsoft.EntityFrameworkCore;
using miniTodo.Api.Configuration;
using miniTodo.Api.Options;
using miniTodo.Api.Services;
using miniTodo.Data;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Services
services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddAppAuthentication(builder.Configuration);

services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlite(connectionString);
});

services.AddSingleton<JwtSettings>();
services.AddSingleton<UserService>();

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

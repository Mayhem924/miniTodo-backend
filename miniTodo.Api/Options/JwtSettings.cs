namespace miniTodo.Api.Options;

public class JwtSettings
{
    public string Secret { get; init; }

	public JwtSettings(IConfiguration configuration)
	{
		Secret = configuration["JwtSettings:Secret"];
	}
}

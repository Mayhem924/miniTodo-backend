using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace miniTodo.Api.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddAppSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "miniTodo Backend", Version = "v1" });

            var securityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Bearer Token",
                Name = "Authorization",
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http
            };

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                        In = ParameterLocation.Header,
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }

    public static WebApplication UseAppSwagger(this WebApplication app, IConfiguration configuration)
    {
        var swaggerEnable = Convert.ToBoolean(configuration["General:EnableSwagger"]);
        if (swaggerEnable)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}

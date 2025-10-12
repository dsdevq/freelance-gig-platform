using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Shared.WebApi.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, string serviceName = "API", string version = "v1")
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new OpenApiInfo { Title = serviceName, Version = version });
            
            // Define JWT Bearer scheme
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT token like: Bearer <your_token>"
            });

            // Require JWT for all endpoints in Swagger
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        
        return services;
    }

    public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app, string serviceName = "API", string version = "v1")
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => 
        { 
            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{serviceName} {version.ToUpper()}"); 
        });

        return app;
    }
}

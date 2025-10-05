using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.WebApi.Handlers;

namespace Shared.WebApi.Extensions;

public static class ErrorHandlingExtensions
{
    public static IServiceCollection AddSharedErrorHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        
        return services;
    }

    public static IApplicationBuilder UseSharedErrorHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
        
        return app;
    }
}

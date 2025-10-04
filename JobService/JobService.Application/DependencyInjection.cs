using JobService.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JobService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJobService, Services.JobService>();
        
        return services;
    }
}


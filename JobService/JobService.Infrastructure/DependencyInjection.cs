using JobService.Application.Common.Interfaces;
using JobService.Infrastructure.Persistence;
using JobService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<JobDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}


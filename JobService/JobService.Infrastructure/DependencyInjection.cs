using JobService.Application.Common.Interfaces;
using JobService.Infrastructure.Persistence;
using JobService.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Extensions;

namespace JobService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgresDbContext<JobDbContext>(configuration, "DefaultConnection");
        services.AddUnitOfWork<UnitOfWork>();

        services.AddScoped<IJobRepository, JobRepository>();

        return services;
    }
}


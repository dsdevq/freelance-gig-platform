using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Persistence;

namespace Shared.Infrastructure.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddSharedUnitOfWork<TUnitOfWork>(this IServiceCollection services)
        where TUnitOfWork : class, IUnitOfWork
    {
        services.AddScoped<IUnitOfWork, TUnitOfWork>();
        return services;
    }

    public static IServiceCollection AddPostgresDbContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(connectionStringName)));

        return services;
    }
}

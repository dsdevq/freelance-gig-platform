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
}

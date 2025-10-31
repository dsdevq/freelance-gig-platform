using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Outbox.Interfaces;
using Shared.Outbox.Options;
using Shared.Outbox.Repositories;
using Shared.Outbox.Services;

namespace Shared.Outbox.Extensions;

public static class OutboxExtensions
{
    public static IServiceCollection AddOutbox<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string configSectionName = "Outbox") 
        where TContext : DbContext
    {
        var outboxOptions = new OutboxOptions();
        configuration.GetSection(configSectionName).Bind(outboxOptions);
        services.Configure<OutboxOptions>(configuration.GetSection(configSectionName));

        services.AddScoped<IOutboxRepository, OutboxRepository<TContext>>();

        if (outboxOptions.Enabled)
        {
            services.AddHostedService(provider => 
                new OutboxBackgroundService(
                    provider,
                    provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<OutboxBackgroundService>>(),
                    outboxOptions.ProcessingInterval,
                    outboxOptions.BatchSize));
        }

        return services;
    }

    public static void ApplyOutboxConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OutboxExtensions).Assembly);
    }
}


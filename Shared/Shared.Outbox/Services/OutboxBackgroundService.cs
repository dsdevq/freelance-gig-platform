using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Outbox.Interfaces;

namespace Shared.Outbox.Services;

public class OutboxBackgroundService(
    IServiceProvider serviceProvider,
    ILogger<OutboxBackgroundService> logger,
    TimeSpan? interval = null,
    int batchSize = 20)
    : BackgroundService
{
    private readonly TimeSpan _interval = interval ?? TimeSpan.FromSeconds(10);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Outbox Background Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing outbox messages");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        logger.LogInformation("Outbox Background Service stopped");
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        
        var repository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var processor = scope.ServiceProvider.GetRequiredService<IOutboxProcessor>();

        var messages = await repository.GetUnprocessedMessagesAsync(batchSize, cancellationToken);

        if (messages.Count == 0)
        {
            return;
        }

        logger.LogInformation("Processing {Count} outbox messages", messages.Count);

        foreach (var message in messages)
        {
            try
            {
                await processor.ProcessAsync(message, cancellationToken);
                await repository.MarkAsProcessedAsync(message.Id, cancellationToken);
                
                logger.LogInformation("Successfully processed outbox message {MessageId} of type {MessageType}", 
                    message.Id, message.Type);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process outbox message {MessageId} of type {MessageType}", 
                    message.Id, message.Type);
                
                await repository.MarkAsFailedAsync(message.Id, ex.Message, cancellationToken);
            }
        }
    }
}


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Outbox.Interfaces;

namespace Shared.Outbox.Services;

public class OutboxBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxBackgroundService> _logger;
    private readonly TimeSpan _interval;
    private readonly int _batchSize;

    public OutboxBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<OutboxBackgroundService> logger,
        TimeSpan? interval = null,
        int batchSize = 20)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _interval = interval ?? TimeSpan.FromSeconds(10);
        _batchSize = batchSize;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Background Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing outbox messages");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Outbox Background Service stopped");
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var repository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var processor = scope.ServiceProvider.GetRequiredService<IOutboxProcessor>();

        var messages = await repository.GetUnprocessedMessagesAsync(_batchSize, cancellationToken);

        if (messages.Count == 0)
        {
            return;
        }

        _logger.LogInformation("Processing {Count} outbox messages", messages.Count);

        foreach (var message in messages)
        {
            try
            {
                await processor.ProcessAsync(message, cancellationToken);
                await repository.MarkAsProcessedAsync(message.Id, cancellationToken);
                
                _logger.LogInformation("Successfully processed outbox message {MessageId} of type {MessageType}", 
                    message.Id, message.Type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process outbox message {MessageId} of type {MessageType}", 
                    message.Id, message.Type);
                
                await repository.MarkAsFailedAsync(message.Id, ex.Message, cancellationToken);
            }
        }
    }
}


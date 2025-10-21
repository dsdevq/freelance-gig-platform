using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.Outbox.Entities;
using Shared.Outbox.Interfaces;

namespace JobService.Infrastructure.Messaging;

public class RabbitMqOutboxProcessor : IOutboxProcessor, IDisposable
{
    private readonly ILogger<RabbitMqOutboxProcessor> _logger;
    private readonly RabbitMqOptions _options;
    private readonly object _lock = new();
    private IConnection? _connection;
    private IModel? _channel;
    private bool _disposed;

    public RabbitMqOutboxProcessor(
        ILogger<RabbitMqOutboxProcessor> logger,
        IOptions<RabbitMqOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public Task ProcessAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(RabbitMqOutboxProcessor));
        }

        EnsureConnection();

        lock (_lock)
        {
            if (_channel is null || !_channel.IsOpen)
            {
                throw new InvalidOperationException("RabbitMQ channel is not initialized or closed");
            }

            var body = Encoding.UTF8.GetBytes(message.Content);
            var routingKey = message.Type.ToLowerInvariant().Replace("event", "");

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.MessageId = message.Id.ToString();
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _channel.BasicPublish(
                exchange: _options.ExchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body);

            _logger.LogInformation(
                "Published message {MessageId} of type {MessageType} to RabbitMQ with routing key {RoutingKey}",
                message.Id, message.Type, routingKey);
        }

        return Task.CompletedTask;
    }

    private void EnsureConnection()
    {
        if (_connection is { IsOpen: true } && _channel is { IsOpen: true })
        {
            return;
        }

        lock (_lock)
        {
            if (_connection is { IsOpen: true } && _channel is { IsOpen: true })
            {
                return;
            }

            CleanupConnection();

            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _options.HostName,
                    Port = _options.Port,
                    UserName = _options.UserName,
                    Password = _options.Password,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                    RequestedHeartbeat = TimeSpan.FromSeconds(60),
                    DispatchConsumersAsync = true
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(
                    exchange: _options.ExchangeName,
                    type: _options.ExchangeType,
                    durable: true,
                    autoDelete: false,
                    arguments: null);

                _logger.LogInformation("Connected to RabbitMQ at {HostName}:{Port}", _options.HostName, _options.Port);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to RabbitMQ at {HostName}:{Port}", _options.HostName, _options.Port);
                throw;
            }
        }
    }

    private void CleanupConnection()
    {
        try
        {
            _channel?.Close();
            _channel?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while closing channel");
        }

        try
        {
            _connection?.Close();
            _connection?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while closing connection");
        }

        _channel = null;
        _connection = null;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            CleanupConnection();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}


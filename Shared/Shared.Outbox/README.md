# Shared.Outbox

Implementation of the Transactional Outbox pattern for reliable message publishing in microservices.

## Overview

The Outbox pattern ensures reliable message/event publishing by:
1. Storing messages in a database table within the same transaction as business operations
2. Processing messages asynchronously via a background service
3. Marking messages as processed after successful publishing

## Components

### Entities
- **OutboxMessage**: Represents a message stored in the outbox table

### Repositories
- **IOutboxRepository**: Interface for outbox message operations
- **OutboxRepository<TContext>**: Generic implementation for any DbContext

### Services
- **OutboxBackgroundService**: Background service that processes outbox messages periodically
- **IOutboxProcessor**: Interface to implement custom message processing logic

### Configuration
- **OutboxMessageConfiguration**: EF Core configuration for OutboxMessage entity
- **OutboxOptions**: Configuration options for outbox processing

## Usage

### 1. Add to your DbContext

```csharp
public class YourDbContext : DbContext
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyOutboxConfiguration();
    }
}
```

### 2. Implement IOutboxProcessor

```csharp
public class YourOutboxProcessor : IOutboxProcessor
{
    private readonly IMessageBroker _messageBroker;

    public YourOutboxProcessor(IMessageBroker messageBroker)
    {
        _messageBroker = messageBroker;
    }

    public async Task ProcessAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        await _messageBroker.PublishAsync(message.Type, message.Content, cancellationToken);
    }
}
```

### 3. Register services

```csharp
builder.Services.AddOutbox<YourDbContext>(builder.Configuration);
builder.Services.AddScoped<IOutboxProcessor, YourOutboxProcessor>();
```

### 4. Add configuration to appsettings.json

```json
{
  "Outbox": {
    "Enabled": true,
    "ProcessingInterval": "00:00:10",
    "BatchSize": 20
  }
}
```

### 5. Save messages to outbox

```csharp
public async Task CreateJobAsync(CreateJobModel model, CancellationToken ct)
{
    using var transaction = await _context.Database.BeginTransactionAsync(ct);
    
    try
    {
        var job = new Job { /* ... */ };
        _context.Jobs.Add(job);
        
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = "JobCreated",
            Content = JsonSerializer.Serialize(new JobCreatedEvent(job.Id)),
            OccurredOnUtc = DateTime.UtcNow
        };
        
        _context.OutboxMessages.Add(outboxMessage);
        
        await _context.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }
    catch
    {
        await transaction.RollbackAsync(ct);
        throw;
    }
}
```

## Configuration Options

- **Enabled**: Enable/disable outbox processing (default: true)
- **ProcessingInterval**: How often to check for unprocessed messages (default: 10 seconds)
- **BatchSize**: Number of messages to process in each batch (default: 20)


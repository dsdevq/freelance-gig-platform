using Microsoft.EntityFrameworkCore;
using Shared.Outbox.Entities;
using Shared.Outbox.Interfaces;

namespace Shared.Outbox.Repositories;

public class OutboxRepository<TContext>(TContext context) : IOutboxRepository
    where TContext : DbContext
{
    public async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        return await context.Set<OutboxMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsProcessedAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var message = await context.Set<OutboxMessage>()
            .FirstOrDefaultAsync(x => x.Id == messageId, cancellationToken);

        if (message != null)
        {
            message.ProcessedOnUtc = DateTime.UtcNow;
            message.Error = null;
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task MarkAsFailedAsync(Guid messageId, string error, CancellationToken cancellationToken = default)
    {
        var message = await context.Set<OutboxMessage>()
            .FirstOrDefaultAsync(x => x.Id == messageId, cancellationToken);

        if (message != null)
        {
            message.Error = error;
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        await context.Set<OutboxMessage>().AddAsync(message, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}


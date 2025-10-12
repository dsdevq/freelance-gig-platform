using Microsoft.EntityFrameworkCore;
using Shared.Outbox.Entities;
using Shared.Outbox.Interfaces;

namespace Shared.Outbox.Repositories;

public class OutboxRepository<TContext> : IOutboxRepository where TContext : DbContext
{
    private readonly TContext _context;

    public OutboxRepository(TContext context)
    {
        _context = context;
    }

    public async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        return await _context.Set<OutboxMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsProcessedAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var message = await _context.Set<OutboxMessage>()
            .FirstOrDefaultAsync(x => x.Id == messageId, cancellationToken);

        if (message != null)
        {
            message.ProcessedOnUtc = DateTime.UtcNow;
            message.Error = null;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task MarkAsFailedAsync(Guid messageId, string error, CancellationToken cancellationToken = default)
    {
        var message = await _context.Set<OutboxMessage>()
            .FirstOrDefaultAsync(x => x.Id == messageId, cancellationToken);

        if (message != null)
        {
            message.Error = error;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        await _context.Set<OutboxMessage>().AddAsync(message, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}


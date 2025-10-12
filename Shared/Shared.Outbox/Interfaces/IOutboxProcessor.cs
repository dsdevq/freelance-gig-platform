using Shared.Outbox.Entities;

namespace Shared.Outbox.Interfaces;

public interface IOutboxProcessor
{
    Task ProcessAsync(OutboxMessage message, CancellationToken cancellationToken = default);
}


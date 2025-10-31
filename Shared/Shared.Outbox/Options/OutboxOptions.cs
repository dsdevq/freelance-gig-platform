namespace Shared.Outbox.Options;

public class OutboxOptions
{
    public TimeSpan ProcessingInterval { get; set; } = TimeSpan.FromSeconds(10);
    public int BatchSize { get; set; } = 20;
    public bool Enabled { get; set; } = true;
}


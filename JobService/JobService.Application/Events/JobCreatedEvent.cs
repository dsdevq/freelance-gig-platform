namespace JobService.Application.Events;

public class JobCreatedEvent
{
    public Guid JobId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public Guid ClientId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string[] RequiredSkills { get; set; } = Array.Empty<string>();
    public int EstimatedDurationInDays { get; set; }
    public DateTime CreatedAt { get; set; }
}


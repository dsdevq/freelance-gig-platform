namespace JobService.Application.Models;

public class CreateJobModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Budget { get; set; }
    public Guid ClientId { get; set; }
    public string Category { get; set; }
    public string[] RequiredSkills { get; set; }
    public int EstimatedDurationInDays { get; set; }
}


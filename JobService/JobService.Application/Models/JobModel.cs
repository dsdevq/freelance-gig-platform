using JobService.Domain.Entities;

namespace JobService.Application.Models;

public class JobModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Budget { get; set; }
    public JobStatus Status { get; set; }
    public Guid ClientId { get; set; }
    public Guid? FreelancerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Category { get; set; }
    public string[] RequiredSkills { get; set; }
    public int EstimatedDurationInDays { get; set; }
}


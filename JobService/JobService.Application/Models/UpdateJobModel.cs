using JobService.Domain.Entities;

namespace JobService.Application.Models;

public class UpdateJobModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? Budget { get; set; }
    public JobStatus? Status { get; set; }
    public Guid? FreelancerId { get; set; }
    public string? Category { get; set; }
    public string[]? RequiredSkills { get; set; }
    public int? EstimatedDurationInDays { get; set; }
}


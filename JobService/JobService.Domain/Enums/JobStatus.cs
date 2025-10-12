namespace JobService.Domain.Entities;

public enum JobStatus
{
    Draft = 0,
    Published = 1,
    InProgress = 2,
    UnderReview = 3,
    Completed = 4,
    Cancelled = 5
}


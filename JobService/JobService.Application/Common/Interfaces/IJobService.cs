using JobService.Application.Models;
using JobService.Domain.Entities;

namespace JobService.Application.Common.Interfaces;

public interface IJobService
{
    Task<JobModel?> GetJobByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<JobModel>> GetAllJobsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<JobModel>> GetJobsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<JobModel>> GetJobsByFreelancerIdAsync(Guid freelancerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<JobModel>> GetJobsByStatusAsync(JobStatus status, CancellationToken cancellationToken = default);
    Task<JobModel> CreateJobAsync(CreateJobModel model, CancellationToken cancellationToken = default);
    Task<JobModel> UpdateJobAsync(Guid id, UpdateJobModel model, CancellationToken cancellationToken = default);
    Task<bool> DeleteJobAsync(Guid id, CancellationToken cancellationToken = default);
}


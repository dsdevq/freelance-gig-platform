using JobService.Application.Common.Interfaces;
using JobService.Application.Models;
using JobService.Domain.Entities;
using Shared.Application.Persistence;

namespace JobService.Application.Services;

public class JobService(IJobRepository jobRepository, IUnitOfWork unitOfWork) : IJobService
{
    public async Task<JobModel?> GetJobByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var job = await jobRepository.GetByIdAsync(id, cancellationToken);
        return job == null ? null : MapToModel(job);
    }

    public async Task<IEnumerable<JobModel>> GetAllJobsAsync(CancellationToken cancellationToken = default)
    {
        var jobs = await jobRepository.GetAllAsync(cancellationToken);
        return jobs.Select(MapToModel);
    }

    public async Task<IEnumerable<JobModel>> GetJobsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        var jobs = await jobRepository.GetByClientIdAsync(clientId, cancellationToken);
        return jobs.Select(MapToModel);
    }

    public async Task<IEnumerable<JobModel>> GetJobsByFreelancerIdAsync(Guid freelancerId, CancellationToken cancellationToken = default)
    {
        var jobs = await jobRepository.GetByFreelancerIdAsync(freelancerId, cancellationToken);
        return jobs.Select(MapToModel);
    }

    public async Task<IEnumerable<JobModel>> GetJobsByStatusAsync(JobStatus status, CancellationToken cancellationToken = default)
    {
        var jobs = await jobRepository.GetByStatusAsync(status, cancellationToken);
        return jobs.Select(MapToModel);
    }

    public async Task<JobModel> CreateJobAsync(CreateJobModel model, CancellationToken cancellationToken = default)
    {
        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            Description = model.Description,
            Budget = model.Budget,
            ClientId = model.ClientId,
            Category = model.Category,
            RequiredSkills = model.RequiredSkills,
            EstimatedDurationInDays = model.EstimatedDurationInDays,
            Status = JobStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdJob = await jobRepository.CreateAsync(job, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToModel(createdJob);
    }

    public async Task<JobModel> UpdateJobAsync(Guid id, UpdateJobModel model, CancellationToken cancellationToken = default)
    {
        var job = await jobRepository.GetByIdAsync(id, cancellationToken);
        if (job == null)
            throw new InvalidOperationException($"Job with ID {id} not found");

        if (model.Title != null) job.Title = model.Title;
        if (model.Description != null) job.Description = model.Description;
        if (model.Budget.HasValue) job.Budget = model.Budget.Value;
        if (model.Status.HasValue) job.Status = model.Status.Value;
        if (model.FreelancerId.HasValue) job.FreelancerId = model.FreelancerId.Value;
        if (model.Category != null) job.Category = model.Category;
        if (model.RequiredSkills != null) job.RequiredSkills = model.RequiredSkills;
        if (model.EstimatedDurationInDays.HasValue) job.EstimatedDurationInDays = model.EstimatedDurationInDays.Value;

        job.UpdatedAt = DateTime.UtcNow;

        if (model.Status == JobStatus.Completed)
            job.CompletedAt = DateTime.UtcNow;

        var updatedJob = await jobRepository.UpdateAsync(job, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToModel(updatedJob);
    }

    public async Task<bool> DeleteJobAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await jobRepository.DeleteAsync(id, cancellationToken);
        if (result)
            await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    private static JobModel MapToModel(Job job)
    {
        return new JobModel
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Budget = job.Budget,
            Status = job.Status,
            ClientId = job.ClientId,
            FreelancerId = job.FreelancerId,
            CreatedAt = job.CreatedAt,
            UpdatedAt = job.UpdatedAt,
            CompletedAt = job.CompletedAt,
            Category = job.Category,
            RequiredSkills = job.RequiredSkills,
            EstimatedDurationInDays = job.EstimatedDurationInDays
        };
    }
}


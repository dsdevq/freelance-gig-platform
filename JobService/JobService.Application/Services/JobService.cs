using AutoMapper;
using FluentValidation;
using JobService.Application.Common.Interfaces;
using JobService.Application.Models;
using JobService.Domain.Entities;
using Shared.Application.Persistence;

namespace JobService.Application.Services;

public class JobService(
    IJobRepository jobRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateJobModel> createValidator,
    IValidator<UpdateJobModel> updateValidator
) : IJobService
{
    public async Task<JobModel?> GetJobByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var job = await jobRepository.GetByIdAsync(id, cancellationToken);
        return job is null ? null : mapper.Map<JobModel>(job);
    }

    public async Task<IEnumerable<JobModel>> GetAllJobsAsync(CancellationToken cancellationToken = default)
    {
        var jobs = await jobRepository.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<JobModel>>(jobs);
    }

    public async Task<IEnumerable<JobModel>> GetJobsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        var jobs = await jobRepository.GetByClientIdAsync(clientId, cancellationToken);
        return mapper.Map<IEnumerable<JobModel>>(jobs);
    }

    public async Task<IEnumerable<JobModel>> GetJobsByFreelancerIdAsync(Guid freelancerId, CancellationToken cancellationToken = default)
    {
        var jobs = await jobRepository.GetByFreelancerIdAsync(freelancerId, cancellationToken);
        return mapper.Map<IEnumerable<JobModel>>(jobs);
    }

    public async Task<IEnumerable<JobModel>> GetJobsByStatusAsync(JobStatus status, CancellationToken cancellationToken = default)
    {
        var jobs = await jobRepository.GetByStatusAsync(status, cancellationToken);
        return mapper.Map<IEnumerable<JobModel>>(jobs);
    }

    public async Task<JobModel> CreateJobAsync(CreateJobModel model, CancellationToken cancellationToken = default)
    {
        await createValidator.ValidateAndThrowAsync(model, cancellationToken);

        var job = mapper.Map<Job>(model);
        await jobRepository.CreateAsync(job, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<JobModel>(job);
    }

    public async Task<JobModel> UpdateJobAsync(Guid id, UpdateJobModel model, CancellationToken cancellationToken = default)
    {
        await updateValidator.ValidateAndThrowAsync(model, cancellationToken);

        var job = await jobRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Job with ID {id} not found.");

        mapper.Map(model, job); // updates only non-null fields
        job.UpdatedAt = DateTime.UtcNow;

        if (model.Status == JobStatus.Completed)
            job.CompletedAt = DateTime.UtcNow;

        await jobRepository.UpdateAsync(job, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<JobModel>(job);
    }

    public async Task<bool> DeleteJobAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await jobRepository.DeleteAsync(id, cancellationToken);
        if (result)
            await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}

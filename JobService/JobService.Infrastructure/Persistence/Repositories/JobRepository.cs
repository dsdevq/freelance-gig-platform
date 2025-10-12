using JobService.Application.Common.Interfaces;
using JobService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobService.Infrastructure.Persistence.Repositories;

public class JobRepository : IJobRepository
{
    private readonly JobDbContext _context;

    public JobRepository(JobDbContext context)
    {
        _context = context;
    }

    public async Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Job>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Jobs.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Job>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Where(j => j.ClientId == clientId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Job>> GetByFreelancerIdAsync(Guid freelancerId, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Where(j => j.FreelancerId == freelancerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Job>> GetByStatusAsync(JobStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Where(j => j.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<Job> CreateAsync(Job job, CancellationToken cancellationToken = default)
    {
        await _context.Jobs.AddAsync(job, cancellationToken);
        return job;
    }

    public async Task<Job> UpdateAsync(Job job, CancellationToken cancellationToken = default)
    {
        _context.Jobs.Update(job);
        return await Task.FromResult(job);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var job = await GetByIdAsync(id, cancellationToken);
        if (job == null)
            return false;

        _context.Jobs.Remove(job);
        return true;
    }
}


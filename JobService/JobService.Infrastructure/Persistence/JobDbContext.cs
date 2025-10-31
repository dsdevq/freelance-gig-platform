using JobService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Outbox.Entities;
using Shared.Outbox.Extensions;

namespace JobService.Infrastructure.Persistence;

public class JobDbContext(DbContextOptions<JobDbContext> options) : DbContext(options)
{
    public DbSet<Job> Jobs { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(JobDbContext).Assembly);
        modelBuilder.ApplyOutboxConfiguration();
    }
}


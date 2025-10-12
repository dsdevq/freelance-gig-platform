using Shared.Infrastructure.Persistence;

namespace JobService.Infrastructure.Persistence;

public class UnitOfWork(JobDbContext context) : UnitOfWorkBase<JobDbContext>(context)
{
}


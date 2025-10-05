using Microsoft.EntityFrameworkCore;
using Shared.Application.Persistence;

namespace Shared.Infrastructure.Persistence;

public class UnitOfWorkBase<TDbContext>(TDbContext dbContext) : IUnitOfWork 
    where TDbContext : DbContext
{
    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new EfCoreTransaction(transaction);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}

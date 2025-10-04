using Microsoft.EntityFrameworkCore.Storage;
using UserService.Application.Common.Interfaces;

namespace UserService.Infrastructure.Persistence;

public class UnitOfWork(UserDbContext dbContext) : IUnitOfWork
{
    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new EfCoreTransaction(transaction);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
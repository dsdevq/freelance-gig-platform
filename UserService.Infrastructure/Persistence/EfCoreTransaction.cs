using Microsoft.EntityFrameworkCore.Storage;
using UserService.Application.Common.Interfaces;

namespace UserService.Infrastructure.Persistence;

public class EfCoreTransaction(IDbContextTransaction efTransaction) : ITransaction
{
    public async Task CommitAsync(CancellationToken cancellationToken = default) 
        => await efTransaction.CommitAsync(cancellationToken);

    public async Task RollbackAsync(CancellationToken cancellationToken = default) 
        => await efTransaction.RollbackAsync(cancellationToken);

    public async ValueTask DisposeAsync() 
        => await efTransaction.DisposeAsync();
}

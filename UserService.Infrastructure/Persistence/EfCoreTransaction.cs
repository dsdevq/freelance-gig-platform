using Microsoft.EntityFrameworkCore.Storage;
using UserService.Application.Common.Interfaces;

namespace UserService.Infrastructure.Persistence;

public class EfCoreTransaction : ITransaction
{
    private readonly IDbContextTransaction _efTransaction;

    public EfCoreTransaction(IDbContextTransaction efTransaction)
    {
        _efTransaction = efTransaction;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default) 
        => await _efTransaction.CommitAsync(cancellationToken);

    public async Task RollbackAsync(CancellationToken cancellationToken = default) 
        => await _efTransaction.RollbackAsync(cancellationToken);

    public async ValueTask DisposeAsync() 
        => await _efTransaction.DisposeAsync();
}

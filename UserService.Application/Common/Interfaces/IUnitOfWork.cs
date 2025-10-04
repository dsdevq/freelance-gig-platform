namespace UserService.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task <ITransaction>BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

using Shared.Infrastructure.Persistence;

namespace UserService.Infrastructure.Persistence;

public class UnitOfWork(UserDbContext dbContext) : UnitOfWorkBase<UserDbContext>(dbContext)
{
}
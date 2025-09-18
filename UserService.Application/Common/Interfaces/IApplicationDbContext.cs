using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IApplicationDbContext
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
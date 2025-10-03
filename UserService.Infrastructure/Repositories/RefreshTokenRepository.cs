using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Repositories;

public class RefreshTokenRepository(UserDbContext dbContext) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task<(Guid UserId, string Email, string UserName, string Role)?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.Id, u.Email, u.UserName })
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return null;
        }

        var role = await dbContext.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Join(dbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
            .FirstOrDefaultAsync(cancellationToken);

        if (role == null)
        {
            return null;
        }

        return (user.Id, user.Email!, user.UserName!, role);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}


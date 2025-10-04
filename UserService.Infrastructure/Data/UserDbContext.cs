using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Infrastructure.Entities;
using UserService.Infrastructure.Identity;

namespace UserService.Infrastructure;

public class UserDbContext(DbContextOptions<UserDbContext> options)
    : IdentityDbContext<AppIdentityUser, AppIdentityRole, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
    }
}
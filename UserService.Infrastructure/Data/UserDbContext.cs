using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Entities;
using UserService.Infrastructure.Identity;

namespace UserService.Infrastructure;

public class UserDbContext(DbContextOptions<UserDbContext> options)
    : IdentityDbContext<AppIdentityUser, AppIdentityRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
    }
}
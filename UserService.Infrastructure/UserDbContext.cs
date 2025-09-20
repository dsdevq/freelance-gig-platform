using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Entities;

namespace UserService.Infrastructure;

public class UserDbContext(DbContextOptions<UserDbContext> options)
    : IdentityDbContext<AppIdentityUser, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
    }
}
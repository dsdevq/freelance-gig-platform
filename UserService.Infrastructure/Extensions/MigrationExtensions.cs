using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Application.Common.Interfaces;

namespace UserService.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        
        
        await dbContext.Database.MigrateAsync();
        var roleService = scope.ServiceProvider.GetRequiredService<IDataSeederService>();
        await roleService.SeedDataAsync();
    }
}
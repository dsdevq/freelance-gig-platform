using JobService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobService.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();
        
        await dbContext.Database.MigrateAsync();
    }
}


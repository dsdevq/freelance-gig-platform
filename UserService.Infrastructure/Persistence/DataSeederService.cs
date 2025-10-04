using UserService.Application.Common.Interfaces;

namespace UserService.Infrastructure.Services;

public class DataSeederService(IRoleService roleService) : IDataSeederService
{
    public async Task SeedDataAsync()
    {
        await roleService.SeedRolesAsync();
    }
}
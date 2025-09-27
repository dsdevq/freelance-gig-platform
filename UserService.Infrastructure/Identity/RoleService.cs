using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Interfaces;
using UserService.Domain.Constants;
using UserService.Infrastructure.Identity;

namespace UserService.Infrastructure.Services;

public class RoleService(RoleManager<AppIdentityRole> roleManager): IRoleService
{
    public async Task SeedRolesAsync()
    {
        var roles = new[] { Roles.Freelancer, Roles.Client };
        
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppIdentityRole { Name = role });
            }
        }
    }
} 
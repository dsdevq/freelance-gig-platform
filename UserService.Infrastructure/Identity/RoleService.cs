using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Identity;

public class RoleService(RoleManager<UserRole> roleManager): IRoleService
{
    public async Task SeedRolesAsync()
    {
        var roles = new[] { Roles.Freelancer, Roles.Client };
        
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new UserRole { Name = role });
            }
        }
    }
} 
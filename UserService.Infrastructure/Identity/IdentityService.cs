using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Interfaces;
using UserService.Infrastructure.Entities;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Identity;

public class IdentityService(
    UserManager<AppIdentityUser> userManager,
    SignInManager<AppIdentityUser> signInManager,
    RoleManager<IdentityRole<Guid>> roleManager)
    : IIdentityService
{
    public async Task<ApplicationUser> RegisterAsync(string email, string password, string fullName, CancellationToken cancellationToken = default)
    {
        var user = new AppIdentityUser 
        { 
            UserName = email, 
            Email = email,
            FullName = fullName
        };
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

        return new ApplicationUser(user.Id, email, fullName);
    }

    public async Task<ApplicationUser?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        if (!result.Succeeded) return null;

        return new ApplicationUser(user.Id, user.Email, user.FullName);
    }
}
using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities;
using UserService.Infrastructure.Entities;
using UserService.Application.Interfaces;

namespace UserService.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;


public class UserService(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
    : IUserService
{
    public async Task<User> SignUpAsync(string email, string password, string fullName)
    {
        var identityUser = new AppIdentityUser
        {
            UserName = email,
            Email = email,
            FullName = fullName
        };

        var result = await userManager.CreateAsync(identityUser, password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        // Map to Domain User
        return new User(identityUser.Id, fullName, email);
    }

    public async Task<User?> SignInAsync(string email, string password)
    {
        var identityUser = await userManager.FindByEmailAsync(email);
        if (identityUser == null) return null;

        var result = await signInManager.CheckPasswordSignInAsync(identityUser, password, lockoutOnFailure: false);
        if (!result.Succeeded) return null;

        return new User(identityUser.Id, identityUser.FullName, identityUser.Email);
        }
}

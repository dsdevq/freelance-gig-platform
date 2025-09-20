using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using UserService.Infrastructure.Entities;

namespace UserService.Infrastructure.Identity;

public class IdentityService(UserManager<AppIdentityUser> userManager,
    SignInManager<AppIdentityUser> signInManager) : IIdentityService
{
    public async Task<UserModel> SignInAsync(SignInModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user is null) 
            throw new LoginFailedException("Invalid email or password");

        var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new LoginFailedException("Invalid email or password");
        
        return new UserModel(user.Email!, user.FullName);
    }

    public async Task<UserModel> SignUpAsync(SignUpModel model)
    {
        AppIdentityUser user = new()
        {
            Email = model.Email,
            FullName = model.FullName,
            UserName = model.FullName,
        };
        var result = await userManager.CreateAsync(user, model.Password);
        
        if (!result.Succeeded)
            throw new SignUpFailedException(string.Join("; ", result.Errors.Select(e => e.Description)));

        return new UserModel(user.Email, user.FullName);
    }
}
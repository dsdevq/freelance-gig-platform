using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities;
using UserService.Application.Common.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Exceptions;
using UserService.Domain.Entities;
using UserService.Infrastructure.Entities;

namespace UserService.Application.Services;

public class UserService(
    UserManager<AppIdentityUser> userManager,
    SignInManager<AppIdentityUser> signInManager,
    IJwtService jwtService
) : IUserService
{
    public async Task<AuthModel> RegisterUserAsync(CreateUserModel model, CancellationToken cancellationToken)
    {
        var user = model.ToModel();
        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            throw new SignUpFailedException(string.Join("; ", result.Errors.Select(e => e.Description)));


        var jwt = jwtService.GenerateToken(user);
        return new AuthModel
        {
            AccessToken = jwt
        };

    }

    public async Task<AuthModel> SignInAsync(LoginModel model, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null) 
            throw new LoginFailedException("Invalid email or password");

        var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new LoginFailedException("Invalid email or password");
        
        var jwt = jwtService.GenerateToken(user);
        return new AuthModel()
        {
            AccessToken = jwt
        };
    }

}

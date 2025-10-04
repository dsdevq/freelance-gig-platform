using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Exceptions;
using UserService.Infrastructure.Entities;

namespace UserService.Infrastructure.Identity;

public class IdentityService(
    UserManager<AppIdentityUser> userManager,
    SignInManager<AppIdentityUser> signInManager,
    UserDbContext userDbContext
) : IIdentityService
{
    public async Task<UserModel> SignInAsync(SignInModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user is null) 
            throw new LoginFailedException("Invalid email or password");

        var result = await signInManager.CheckPasswordSignInAsync(
            user, model.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
            throw new LoginFailedException("Invalid email or password");

        var roles = await userManager.GetRolesAsync(user);
        var parsedRole = ParseRole(roles);

        return new UserModel(user.Email!, user.UserName!, parsedRole)
        {
            Id = user.Id
        };
    }

    public async Task<UserModel> SignUpAsync(SignUpModel model, RoleType roleName)
    {
        await using var transaction = await userDbContext.Database.BeginTransactionAsync();

        var user = new AppIdentityUser
        {
            Email = model.Email,
            UserName = model.Name
        };

        var createUser = await userManager.CreateAsync(user, model.Password);
        if (!createUser.Succeeded)
        {
            await transaction.RollbackAsync();
            throw new SignUpFailedException(string.Join("; ", createUser.Errors.Select(e => e.Description)));
        }

        var assignRoleResult = await userManager.AddToRoleAsync(user, roleName.ToString());
        if (!assignRoleResult.Succeeded)
        {
            await transaction.RollbackAsync();
            throw new SignUpFailedException(string.Join("; ", assignRoleResult.Errors.Select(e => e.Description)));
        }

        await transaction.CommitAsync();

        var roles = await userManager.GetRolesAsync(user);
        var parsedRole = ParseRole(roles);

        return new UserModel(user.Email!, user.UserName!, parsedRole)
        {
            Id = user.Id
        };
    }

    public async Task<UserModel> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            throw new UnauthorizedAccessException("User not found");

        var roles = await userManager.GetRolesAsync(user);
        if (!roles.Any())
            throw new UnauthorizedAccessException("User not found");

        var parsedRole = ParseRole(roles);

        return new UserModel(user.Email!, user.UserName!, parsedRole)
        {
            Id = user.Id
        };
    }

    private static RoleType ParseRole(IList<string> roles)
    {
        return Enum.Parse<RoleType>(roles.First());
    }
}

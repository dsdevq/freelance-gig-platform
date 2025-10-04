using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Exceptions;

namespace UserService.Infrastructure.Identity;

public class IdentityService(UserManager<User> userManager,
    SignInManager<User> signInManager,
    IUnitOfWork unitOfWork
    ) : IIdentityService
{
    public async Task<UserModel> SignInAsync(SignInModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user is null) 
            throw new LoginFailedException("Invalid email or password");

        var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new LoginFailedException("Invalid email or password");
        
        var roles = await userManager.GetRolesAsync(user);
        var parsedRole = Enum.Parse<RoleType>(roles[0]);
        user.Role = parsedRole;
        
        return new UserModel
        {
            Role = user.Role,
            Email = user.Email,
            UserName = user.UserName,
            Id =  user.Id
        };
    }

    public async Task<UserModel> SignUpAsync(SignUpModel model, RoleType roleName, CancellationToken cancellationToken)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName
            };

            var createUser = await userManager.CreateAsync(user, model.Password);
            if (!createUser.Succeeded)
                throw new SignUpFailedException(string.Join("; ", createUser.Errors.Select(e => e.Description)));

            var assignRoleResult = await userManager.AddToRoleAsync(user, roleName.ToString());
            if (!assignRoleResult.Succeeded)
                throw new SignUpFailedException(string.Join("; ", assignRoleResult.Errors.Select(e => e.Description)));

            await transaction.CommitAsync(cancellationToken);

            var roles = await userManager.GetRolesAsync(user);
            var parsedRole = Enum.Parse<RoleType>(roles.First());
            user.Role = parsedRole;

            return new UserModel
            {
                Role = user.Role,
                Email = user.Email,
                UserName = user.UserName,
                Id =  user.Id
            };

        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<UserModel> GetUserByIdAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());


        if (user is null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var roles = await userManager.GetRolesAsync(user);

        if (!roles.Any())
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var parsedRole = Enum.Parse<RoleType>(roles.First());
        user.Role = parsedRole;
        
        return new UserModel
        {
            Role = user.Role,
            Email = user.Email,
            UserName = user.UserName,
            Id =  user.Id
        };
    }
}
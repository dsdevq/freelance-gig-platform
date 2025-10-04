using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.Common.Interfaces;

public interface IIdentityService
{
    public Task<UserModel> SignInAsync(SignInModel model);
    public Task<UserModel> SignUpAsync(SignUpModel model, RoleType roleName, CancellationToken cancellationToken);
    public Task<UserModel> GetUserByIdAsync(Guid userId);
}
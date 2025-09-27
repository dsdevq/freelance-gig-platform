using UserService.Application.Models;
using UserService.Domain.Enums;

namespace UserService.Application.Common.Interfaces;

public interface IUserService
{
    Task<AuthModel> SignUpAsync(SignUpModel model, RoleType role, CancellationToken cancellationToken);
    Task<AuthModel> SignInAsync(SignInModel model, CancellationToken cancellationToken);
}
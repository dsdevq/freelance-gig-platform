using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Interfaces;

public interface IUserService
{
Task<AuthModel> RegisterUserAsync(CreateUserModel model, CancellationToken cancellationToken);
    Task<AuthModel> SignInAsync(LoginModel model, CancellationToken cancellationToken);
}
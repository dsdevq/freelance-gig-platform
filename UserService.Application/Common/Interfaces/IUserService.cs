using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Interfaces;

public interface IUserService
{
    Task<AuthModel> SignUpAsync(SignUpModel model, CancellationToken cancellationToken);
    Task<AuthModel> SignInAsync(SignInModel model, CancellationToken cancellationToken);
}
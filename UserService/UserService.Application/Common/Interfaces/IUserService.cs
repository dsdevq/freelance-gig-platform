using Shared.Domain.Enums;
using UserService.Application.Models;

namespace UserService.Application.Common.Interfaces;

public interface IUserService
{
    Task<AuthModel> SignUpAsync(SignUpModel model, RoleType role, CancellationToken cancellationToken);
    Task<AuthModel> SignInAsync(SignInModel model, CancellationToken cancellationToken);
    Task<AuthModel> RefreshAsync(RefreshTokenModel model, CancellationToken cancellationToken);
}
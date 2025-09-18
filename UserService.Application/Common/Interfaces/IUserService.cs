using UserService.Domain.Entities;

namespace UserService.Application.Common.Interfaces;

public interface IUserService
{
    Task<ApplicationUser> SignUpAsync(string email, string password, string fullName);
    Task<ApplicationUser?> SignInAsync(string email, string password);
}
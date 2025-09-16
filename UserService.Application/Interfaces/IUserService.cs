using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IUserService
{
    Task<User> SignUpAsync(string email, string password, string fullName);
    Task<User?> SignInAsync(string email, string password);
}
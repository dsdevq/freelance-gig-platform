using UserService.Domain.Entities;

namespace UserService.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<ApplicationUser> RegisterAsync(string email, string password, string fullName);
    Task<ApplicationUser?> LoginAsync(string email, string password);
}

using UserService.Domain.Entities;
using UserService.Application.Common.Interfaces;

namespace UserService.Application.Services;

public class UserService(IIdentityService identityService) : IUserService
{
    public async Task<ApplicationUser> SignUpAsync(string email, string password, string fullName)
    {
        return await identityService.RegisterAsync(email, password, fullName);
    }

    public async Task<ApplicationUser?> SignInAsync(string email, string password)
    {
        return await identityService.LoginAsync(email, password);
    }
}

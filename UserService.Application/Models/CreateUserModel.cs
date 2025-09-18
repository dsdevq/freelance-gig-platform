using UserService.Domain.Entities;
using UserService.Infrastructure.Entities;

namespace UserService.Application.Models;

public record CreateUserModel(string Email, string Password, string FullName)
{
    public AppIdentityUser ToModel() => new(){ Email = Email, UserName = FullName};
};

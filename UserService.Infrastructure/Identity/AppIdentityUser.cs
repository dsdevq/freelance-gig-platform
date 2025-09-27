using Microsoft.AspNetCore.Identity;

namespace UserService.Infrastructure.Entities;

public class AppIdentityUser: IdentityUser<Guid>
{
    public string FullName { get; init; } = string.Empty;
}
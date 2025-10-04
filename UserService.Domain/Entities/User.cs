using Microsoft.AspNetCore.Identity;
using UserService.Domain.Enums;

namespace UserService.Domain.Entities;
public class User: IdentityUser<Guid>
{
    public RoleType Role { get; set; }
}
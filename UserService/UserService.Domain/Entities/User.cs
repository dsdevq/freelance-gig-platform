using Microsoft.AspNetCore.Identity;
using Shared.Domain.Enums;

namespace UserService.Domain.Entities;
public class User: IdentityUser<Guid>
{
    public RoleType Role { get; set; }
}
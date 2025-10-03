using UserService.Domain.Constants;
using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public class UserModel(string email, string name, RoleType roles)
{
    public Guid Id { get; set; }
    public string Email { get; private set; } = email;
    public string Name { get; private set; } = name;
    public RoleType Role { get; private set; } = roles;
}
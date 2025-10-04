using UserService.Domain.Enums;

namespace UserService.Application.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public RoleType Role { get; set; }
}
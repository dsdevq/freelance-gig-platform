namespace UserService.Domain.Entities;

public class UserModel(string email, string fullName)
{
    public Guid Id { get; init; }
    public string Email { get; private set; } = email;
    public string FullName { get;init; } = fullName;
}
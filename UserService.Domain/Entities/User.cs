namespace UserService.Domain.Entities;

public class User(Guid id, string email, string fullName)
{
    public Guid Id { get; private set; } = id;
    public string Email { get; private set; } = email;
    public string FullName { get; private set; } = fullName;
}
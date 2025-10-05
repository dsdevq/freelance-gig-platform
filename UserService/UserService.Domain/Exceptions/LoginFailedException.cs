namespace UserService.Domain.Exceptions;

public class LoginFailedException : UnauthorizedAccessException
{
    public LoginFailedException() 
        : base("Login failed. Invalid credentials.") { }

    public LoginFailedException(string message) 
        : base(message) { }

    public LoginFailedException(string message, Exception innerException) 
        : base(message, innerException) { }
}
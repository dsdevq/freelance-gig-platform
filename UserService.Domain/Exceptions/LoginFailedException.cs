namespace UserService.Domain.Exceptions;

public class LoginFailedException(string message) : Exception(message);

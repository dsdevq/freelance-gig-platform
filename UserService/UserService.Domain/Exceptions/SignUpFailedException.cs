namespace UserService.Domain.Exceptions;

public class SignUpFailedException(string error) : Exception(error);
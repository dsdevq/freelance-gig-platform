using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.Exceptions;

public class SignUpFailedException : UnauthorizedAccessException
{
    public IEnumerable<IdentityError> Errors { get; }

    public SignUpFailedException(IEnumerable<IdentityError> errors)
        : base($"Sign up failed: {string.Join("; ", errors.Select(e => e.Description))}")
    {
        Errors = errors.ToList();
    }

    public SignUpFailedException(string message)
        : base(message) { }

    public SignUpFailedException(string message, Exception innerException)
        : base(message, innerException) { }
}

namespace UserService.Application.DTOs;

public record SignUpRequest(string Email, string Password, string FullName);

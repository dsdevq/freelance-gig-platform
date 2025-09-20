using UserService.Application.Common.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Models;

public record SignUpModel(string Email, string Password, string FullName);

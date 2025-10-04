using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(UserModel user);
    string GenerateRefreshToken();
} 
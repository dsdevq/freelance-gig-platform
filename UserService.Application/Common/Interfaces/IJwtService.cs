using UserService.Domain.Entities;

namespace UserService.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
    string GenerateRefreshToken();
} 
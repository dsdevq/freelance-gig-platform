using UserService.Domain.Entities;
using UserService.Infrastructure.Entities;

namespace UserService.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(AppIdentityUser user);
    string GenerateRefreshToken();
} 
using UserService.Application.Common.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.Services;

public class UserService(
    IIdentityService identityService,
    IJwtProvider jwtProvider,
    IRefreshTokenRepository refreshTokenRepository
) : IUserService
{
    public async Task<AuthModel> SignUpAsync(SignUpModel model, RoleType role, CancellationToken cancellationToken)
    {
        var user = await identityService.SignUpAsync(model, role);

        var jwt = jwtProvider.GenerateToken(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        await refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        
        return new AuthModel
        {
            AccessToken = jwt,
            RefreshToken = refreshToken
        };

    }

    public async Task<AuthModel> SignInAsync(SignInModel model, CancellationToken cancellationToken)
    {
        var user = await identityService.SignInAsync(model);
        
        var jwt = jwtProvider.GenerateToken(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        await refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        
        return new AuthModel
        {
            AccessToken = jwt,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthModel> RefreshAsync(RefreshTokenModel model, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(model.RefreshToken, cancellationToken);

        if (refreshToken == null || !refreshToken.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token");
        }

        // Revoke old refresh token
        refreshToken.RevokedAt = DateTime.UtcNow;
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        // Get user details
        var userInfo = await refreshTokenRepository.GetUserByIdAsync(refreshToken.UserId, cancellationToken);

        if (userInfo == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var parsedRole = Enum.Parse<RoleType>(userInfo.Value.Role);
        var userModel = new UserModel(userInfo.Value.Email, userInfo.Value.UserName, parsedRole);

        // Generate new tokens
        var newAccessToken = jwtProvider.GenerateToken(userModel);
        var newRefreshToken = jwtProvider.GenerateRefreshToken();

        var newRefreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userInfo.Value.UserId,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        await refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

        return new AuthModel
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

}

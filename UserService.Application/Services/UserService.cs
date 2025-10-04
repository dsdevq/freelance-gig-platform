using UserService.Application.Common.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.Services;

public class UserService(
    IIdentityService identityService,
    IJwtProvider jwtProvider,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork
) : IUserService
{
    public async Task<AuthModel> SignUpAsync(SignUpModel model, RoleType role, CancellationToken cancellationToken)
    {
        var user = await identityService.SignUpAsync(model, role);

        var jwt = jwtProvider.GenerateToken(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
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
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var refreshToken = await refreshTokenRepository.GetByTokenAsync(model.RefreshToken, cancellationToken);

        if (refreshToken is not { IsActive: true })
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        refreshToken.RevokedAt = DateTime.UtcNow;

        var userModel = await identityService.GetUserByIdAsync(refreshToken.UserId, cancellationToken);

        var newAccessToken = jwtProvider.GenerateToken(userModel);
        var newRefreshToken = jwtProvider.GenerateRefreshToken();

        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = userModel.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        await refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return new AuthModel
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

}

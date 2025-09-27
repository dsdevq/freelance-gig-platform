using UserService.Application.Common.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Enums;

namespace UserService.Application.Services;

public class UserService(
    IIdentityService identityService,
    IJwtProvider jwtProvider
) : IUserService
{
    public async Task<AuthModel> SignUpAsync(SignUpModel model, RoleType role, CancellationToken cancellationToken)
    {
        var user = await identityService.SignUpAsync(model, role);

        var jwt = jwtProvider.GenerateToken(user);
        
        return new AuthModel
        {
            AccessToken = jwt
        };

    }

    public async Task<AuthModel> SignInAsync(SignInModel model, CancellationToken cancellationToken)
    {
        var user = await identityService.SignInAsync(model);
        
        var jwt = jwtProvider.GenerateToken(user);
        
        return new AuthModel
        {
            AccessToken = jwt
        };
    }

}

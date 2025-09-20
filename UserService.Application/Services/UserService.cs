using UserService.Application.Common.Interfaces;
using UserService.Application.Models;

namespace UserService.Application.Services;

public class UserService(
    IIdentityService identityService,
    IJwtService jwtService
) : IUserService
{
    public async Task<AuthModel> SignUpAsync(SignUpModel model, CancellationToken cancellationToken)
    {
        var user = await identityService.SignUpAsync(model);

        var jwt = jwtService.GenerateToken(user);
        
        return new AuthModel
        {
            AccessToken = jwt
        };

    }

    public async Task<AuthModel> SignInAsync(SignInModel model, CancellationToken cancellationToken)
    {
        var user = await identityService.SignInAsync(model);
        
        var jwt = jwtService.GenerateToken(user);
        
        return new AuthModel
        {
            AccessToken = jwt
        };
    }

}

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Infrastructure.Identity;

namespace UserService.API.OptionsSetup;

public class JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions): IConfigureOptions<JwtBearerOptions>
{
    private JwtOptions JwtOptions => jwtOptions.Value;
    
    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtOptions.Issuer,
            ValidAudience = JwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtOptions.SecretKey ))
        };
    }
}
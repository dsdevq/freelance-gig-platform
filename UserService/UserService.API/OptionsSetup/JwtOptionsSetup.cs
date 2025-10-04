using Microsoft.Extensions.Options;
using UserService.Infrastructure.Identity;

namespace UserService.API.OptionsSetup;

public class JwtOptionsSetup(IConfiguration configuration): IConfigureOptions<JwtOptions>
{
    private const string SectionName = "JwtSettings";
    public void Configure(JwtOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
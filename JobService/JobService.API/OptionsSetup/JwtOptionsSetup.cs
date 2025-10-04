using Microsoft.Extensions.Options;
using JobService.Infrastructure.Identity;

namespace JobService.API.OptionsSetup;

public class JwtOptionsSetup(IConfiguration configuration): IConfigureOptions<JwtOptions>
{
    private const string SectionName = "JwtSettings";
    public void Configure(JwtOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}



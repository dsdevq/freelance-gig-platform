using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Common.Interfaces;
using UserService.Infrastructure.Entities;
using UserService.Infrastructure.Identity;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure;


public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // JWT service
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddScoped<IIdentityService, IdentityService>();

        // Database
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("UserDb")));

        // Repositories
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IDataSeederService, DataSeederService>();


        services.AddIdentity<AppIdentityUser, AppIdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<AppIdentityRole>();
        
        services.AddProblemDetails();
    }
}

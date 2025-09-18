using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using UserService.Application.Common.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryHours;

    public JwtService(IConfiguration configuration)
    {
        _secretKey = configuration["JwtSettings:SecretKey"];
        _issuer = configuration["JwtSettings:Issuer"] ;
        _audience = configuration["JwtSettings:Audience"] ;
        _expiryHours = int.Parse(configuration["JwtSettings:ExpiryHours"]);
    }

    public string GenerateToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("jti", Guid.NewGuid().ToString())
            ]),
            Expires = DateTime.UtcNow.AddHours(_expiryHours),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
} 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.API.Handlers;
using UserService.Application.Models;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Entities;
using UserService.Application.Common.Interfaces;
using UserService.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("UserDb")));

builder.Services.AddIdentity<AppIdentityUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserService, UserService.Application.Services.UserService>();
builder.Services.AddScoped<IJwtService, UserService.Infrastructure.Services.JwtService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapPost("/register", async (IUserService userService, IJwtService jwtService, RegisterModel dto) =>
{
    var user = await userService.SignUpAsync(dto.Email, dto.Password, dto.FullName);
    var token = jwtService.GenerateToken(user);
    
    return Results.Ok(new 
    { 
        Token = token
    });
});

app.MapPost("/login", async (IUserService userService, IJwtService jwtService, LoginModel dto) =>
{
    var user = await userService.SignInAsync(dto.Email, dto.Password);
    if (user == null)
        throw new UnauthorizedAccessException("Invalid email or password");

    var token = jwtService.GenerateToken(user);
    
    return Results.Ok(new 
    { 
        Token = token
    });
});

app.Run();
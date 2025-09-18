using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapPost("/register", async (IUserService userService, RegisterModel dto) =>
{
    try
    {
        var user = await userService.SignUpAsync(dto.Email, dto.Password, dto.FullName);
        return Results.Ok(new { UserId = user.Id });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
});

app.MapPost("/login", async (IUserService userService, LoginModel dto) =>
{
    var user = await userService.SignInAsync(dto.Email, dto.Password);
    if (user != null)
        return Results.Ok(new { User = user });
    else
        return Results.Unauthorized();
});

app.Run();
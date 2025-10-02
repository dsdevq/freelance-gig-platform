using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserService.API.Endpoints;
using UserService.API.Extensions;
using UserService.API.Handlers;
using UserService.API.OptionsSetup;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Extensions;
using UserService.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();


builder.Services.AddAuthorization();

builder.Services.AddSwagger(); // <- your extension

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapAuthEndpoints();
app.MapUserEndpoints();

app.MapGet("me", (ClaimsPrincipal claimsPrincipal) =>
{
    return Results.Ok("Result it okay");
}).RequireAuthorization();

// Database
app.ApplyMigrations();

app.Run();

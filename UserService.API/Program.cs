using Microsoft.AspNetCore.Authentication.JwtBearer;
using UserService.API.Endpoints;
using UserService.API.Extensions;
using UserService.API.Handlers;
using UserService.API.OptionsSetup;
using UserService.Application;
using UserService.Domain.Constants;
using UserService.Infrastructure;
using UserService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Roles.Client, policy =>
        policy
            .RequireRole(Roles.Client)
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

builder.Services.AddSwagger(); // <- your extension

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUI(); // <- your extension
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapAuthEndpoints();
app.MapUserEndpoints();

// Database
app.ApplyMigrations();

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using UserService.API.Endpoints;
using UserService.API.Extensions;
using UserService.API.Handlers;
using UserService.API.OptionsSetup;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();




builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddSwagger(); // <- your extension

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

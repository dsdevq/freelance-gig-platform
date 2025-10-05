using Shared.WebApi.Extensions;
using UserService.API.Endpoints;
using UserService.API.Handlers;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddSharedJwtAuthentication();
builder.Services.AddSharedSwagger("UserService API", "v1"); 

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSharedSwagger("UserService API", "v1");
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapAuthEndpoints();
app.MapUserEndpoints();

// Database
await app.ApplyMigrationsAsync();

app.Run();

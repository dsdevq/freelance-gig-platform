using Shared.WebApi.Extensions;
using UserService.API.Endpoints;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSharedErrorHandling();
builder.Services.AddSharedJwtAuthentication();
builder.Services.AddSharedSwagger("UserService API", "v1");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSharedSwagger("UserService API", "v1");
}

app.UseHttpsRedirection();
app.UseSharedErrorHandling();
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapUserEndpoints();

await app.ApplyMigrationsAsync();

app.Run();

using Shared.WebApi.Extensions;
using UserService.API.Endpoints;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddErrorHandling();
builder.Services.AddJwtAuthentication();
builder.Services.AddSwagger("UserService API", "v1");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUI("UserService API", "v1");
}

app.UseHttpsRedirection();
app.UseErrorHandling();
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapUserEndpoints();

await app.ApplyMigrationsAsync();

app.Run();

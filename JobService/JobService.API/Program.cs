using JobService.API.Endpoints;
using JobService.Application;
using JobService.Infrastructure;
using JobService.Infrastructure.Extensions;
using Shared.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddErrorHandling();
builder.Services.AddJwtAuthentication();
builder.Services.AddSwagger("JobService API", "v1");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUI("JobService API", "v1");
}

app.UseHttpsRedirection();
app.UseErrorHandling();
app.UseAuthentication();
app.UseAuthorization();

app.MapJobEndpoints();

await app.ApplyMigrationsAsync();

app.Run();

using JobService.API.Endpoints;
using JobService.Application;
using JobService.Infrastructure;
using Shared.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSharedErrorHandling();
builder.Services.AddSharedJwtAuthentication();
builder.Services.AddSharedSwagger("JobService API", "v1");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSharedSwagger("JobService API", "v1");
}

app.UseHttpsRedirection();
app.UseSharedErrorHandling();
app.UseAuthentication();
app.UseAuthorization();

app.MapJobEndpoints();

app.Run();

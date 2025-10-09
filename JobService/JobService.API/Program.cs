using JobService.API.Endpoints;
using JobService.Application;
using JobService.Infrastructure;
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
    app.UseSwagger("JobService API", "v1");
}

app.UseHttpsRedirection();
app.UseErrorHandling();
app.UseAuthentication();
app.UseAuthorization();

app.MapJobEndpoints();

app.Run();

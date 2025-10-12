using FluentValidation;
using FluentValidation.AspNetCore;
using JobService.Application.Common.Interfaces;
using JobService.Application.Mappings;
using JobService.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace JobService.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJobService, Services.JobService>();
        services.AddAutoMapper(_ => { }, typeof(JobProfile).Assembly);
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateJobModelValidator>();
    }
}


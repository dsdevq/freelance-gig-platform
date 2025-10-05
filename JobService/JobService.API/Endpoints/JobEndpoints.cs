using JobService.API.Constants;
using JobService.Application.Common.Interfaces;
using JobService.Application.Models;
using JobService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Constants;

namespace JobService.API.Endpoints;

public static class JobEndpoints
{
    public static void MapJobEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(JobRoutes.Base).WithTags("Jobs")
            .RequireAuthorization(p => p.RequireRole(Roles.Client));

        group.MapGet(JobRoutes.GetAll, async (IJobService jobService, CancellationToken ct) =>
        {
            var jobs = await jobService.GetAllJobsAsync(ct);
            return Results.Ok(jobs);
        })
        .WithName("GetAllJobs")
        .Produces<IEnumerable<JobModel>>()
        .RequireAuthorization();

        group.MapGet(JobRoutes.GetById, async (Guid id, IJobService jobService, CancellationToken ct) =>
            {
                var job = await jobService.GetJobByIdAsync(id, ct);
                return job != null ? Results.Ok(job) : Results.NotFound();
            })
            .WithName("GetJobById")
            .Produces<JobModel>()
            .Produces(404);

        group.MapGet(JobRoutes.GetByClientId, async (Guid clientId, IJobService jobService, CancellationToken ct) =>
            {
                var jobs = await jobService.GetJobsByClientIdAsync(clientId, ct);
                return Results.Ok(jobs);
            })
            .WithName("GetJobsByClientId")
            .Produces<IEnumerable<JobModel>>();

        group.MapGet(JobRoutes.GetByFreelancerId, async (Guid freelancerId, IJobService jobService, CancellationToken ct) =>
        {
            var jobs = await jobService.GetJobsByFreelancerIdAsync(freelancerId, ct);
            return Results.Ok(jobs);
        })
        .WithName("GetJobsByFreelancerId")
        .Produces<IEnumerable<JobModel>>()
        .RequireAuthorization();

        group.MapGet(JobRoutes.GetByStatus, async (JobStatus status, IJobService jobService, CancellationToken ct) =>
        {
            var jobs = await jobService.GetJobsByStatusAsync(status, ct);
            return Results.Ok(jobs);
        })
        .WithName("GetJobsByStatus")
        .Produces<IEnumerable<JobModel>>();

        group.MapPost(JobRoutes.Create,
                async ([FromBody] CreateJobModel model, IJobService jobService, CancellationToken ct) =>
                {
                    var job = await jobService.CreateJobAsync(model, ct);
                    return Results.Created($"{JobRoutes.Base}/{job.Id}", job);
                })
            .WithName("CreateJob")
            .Produces<JobModel>(201);

        group.MapPut(JobRoutes.Update, async (Guid id, [FromBody] UpdateJobModel model, IJobService jobService, CancellationToken ct) =>
        {
            try
            {
                var job = await jobService.UpdateJobAsync(id, model, ct);
                return Results.Ok(job);
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateJob")
        .Produces<JobModel>()
        .Produces(404);

        group.MapDelete(JobRoutes.Delete, async (Guid id, IJobService jobService, CancellationToken ct) =>
        {
            var result = await jobService.DeleteJobAsync(id, ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteJob")
        .Produces(204)
        .Produces(404);
    }
}


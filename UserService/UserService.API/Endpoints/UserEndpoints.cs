using Shared.Domain.Constants;
using UserService.API.Constants;

namespace UserService.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(UserRoutes.Base).WithTags("Users");

        group.MapGet(UserRoutes.ClientMe, () =>
        {
            return Results.Ok("This is a client me");
        })
        .WithName("GetClientProfile")
        .Produces<string>()
        .RequireAuthorization(p => p.RequireRole(Roles.Client));

        group.MapGet(UserRoutes.FreelancerMe, () =>
        {
            return Results.Ok("This is a freelancer me");
        })
        .WithName("GetFreelancerProfile")
        .Produces<string>()
        .RequireAuthorization(p => p.RequireRole(Roles.Freelancer));
    }
}
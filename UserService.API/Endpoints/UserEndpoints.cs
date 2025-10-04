using UserService.API.Constants;
using UserService.Domain.Constants;

namespace UserService.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(UserRoutes.ClientMe,
            async () =>
            {
                return Results.Ok("This is a client me");
            }).RequireAuthorization(p => p.RequireRole(Roles.Client));
        
        app.MapGet(UserRoutes.FreelancerMe, () =>
    {
            return Results.Ok("thiss is freelancer me");
        }).RequireAuthorization(policy => policy.RequireRole(Roles.Freelancer));

    
    }
}
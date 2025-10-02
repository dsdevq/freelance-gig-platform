using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            }).RequireAuthorization("client-policy");
        
        app.MapGet(UserRoutes.FreelancerMe, () =>
    {
            return Results.Ok();
        }).RequireAuthorization(policy => policy.RequireRole(Roles.Freelancer));

    
    }
}
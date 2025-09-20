using Microsoft.AspNetCore.Mvc;
using UserService.API.Constants;
using UserService.Application.Common.Interfaces;
using UserService.Application.Models;

namespace UserService.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(AuthRoutes.Register,
            async ([FromServices] IUserService userService,
                [FromBody] SignUpModel model,
                CancellationToken cancellationToken) =>
            {
                var result = await userService.SignUpAsync(model, cancellationToken);
                return Results.Ok(result);
            });

        app.MapPost(AuthRoutes.Login,
            async ([FromServices] IUserService userService,
                [FromBody] SignInModel model,
                CancellationToken cancellationToken) =>
            {
                var result = await userService.SignInAsync(model, cancellationToken);
                return Results.Ok(result);
            });
    }
}
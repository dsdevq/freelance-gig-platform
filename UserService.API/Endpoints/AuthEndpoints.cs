using Microsoft.AspNetCore.Mvc;
using UserService.API.Constants;
using UserService.Application.Common.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(AuthRoutes.RegisterClient,
            async ([FromServices] IUserService userService,
                [FromBody] SignUpModel model,
                CancellationToken cancellationToken) =>
            {
                var result = await userService.SignUpAsync(model, RoleType.Client, cancellationToken);
                return Results.Ok(result);
            });
        
        app.MapPost(AuthRoutes.RegisterFreelancer,
            async ([FromServices] IUserService userService,
                [FromBody] SignUpModel model,
                CancellationToken cancellationToken) =>
            {
                var result = await userService.SignUpAsync(model, RoleType.Freelancer, cancellationToken);
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

        app.MapPost(AuthRoutes.Refresh,
            async ([FromServices] IUserService userService,
                [FromBody] RefreshTokenModel model,
                CancellationToken cancellationToken) =>
            {
                var result = await userService.RefreshAsync(model, cancellationToken);
                return Results.Ok(result);
            });
    }
}
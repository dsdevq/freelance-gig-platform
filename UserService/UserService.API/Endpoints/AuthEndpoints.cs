using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enums;
using UserService.API.Constants;
using UserService.Application.Common.Interfaces;
using UserService.Application.Models;

namespace UserService.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(AuthRoutes.Base).WithTags("Authentication");

        group.MapPost(AuthRoutes.RegisterClient, async (
            [FromBody] SignUpModel model,
            IUserService userService,
            CancellationToken ct) =>
        {
            var result = await userService.SignUpAsync(model, RoleType.Client, ct);
            return Results.Ok(result);
        })
        .WithName("RegisterClient")
        .Produces<AuthModel>();

        group.MapPost(AuthRoutes.RegisterFreelancer, async (
            [FromBody] SignUpModel model,
            IUserService userService,
            CancellationToken ct) =>
        {
            var result = await userService.SignUpAsync(model, RoleType.Freelancer, ct);
            return Results.Ok(result);
        })
        .WithName("RegisterFreelancer")
        .Produces<AuthModel>();

        group.MapPost(AuthRoutes.Login, async (
            [FromBody] SignInModel model,
            IUserService userService,
            CancellationToken ct) =>
        {
            var result = await userService.SignInAsync(model, ct);
            return Results.Ok(result);
        })
        .WithName("Login")
        .Produces<AuthModel>();

        group.MapPost(AuthRoutes.Refresh, async (
            [FromBody] RefreshTokenModel model,
            IUserService userService,
            CancellationToken ct) =>
        {
            var result = await userService.RefreshAsync(model, ct);
            return Results.Ok(result);
        })
        .WithName("RefreshToken")
        .Produces<AuthModel>();
    }
}
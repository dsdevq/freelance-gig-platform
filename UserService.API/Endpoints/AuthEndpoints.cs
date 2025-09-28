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
        app.MapPost("generate-token", (IJwtProvider jwtProvider) =>
        {
            var token = jwtProvider.GenerateToken(new UserModel("someemail@email.com", "someName", RoleType.Client));
            
            return Results.Ok(token);
        });
        
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
    }
}
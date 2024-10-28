using CleanDomainValidation.Application;
using DBetter.Application.Users.Commands.RegisterCommand;
using DBetter.Contracts.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DBetter.Api;

public static class UserModule
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (
            IMediator mediator,
            RegisterUserParameters parameters) =>
            {
                var command = Builder<RegisterUserCommand>
                    .BindParameters(parameters)
                    .BuildUsing<RegisterUserRequestBuilder>();

                if (command.HasFailed) return Results.BadRequest();
                
                var user = await mediator.Send(command.Value);
                if(user.HasFailed) return Results.BadRequest();
                
                return Results.Created($"/users/{user.Value.Id}", user.Value);
            })
            .WithName("Register")
            .WithOpenApi();
    }
}
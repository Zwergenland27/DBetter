using System.Security.Claims;
using CleanDomainValidation.Application;
using DBetter.Application.Users.Commands.EditPersonalData;
using DBetter.Contracts.Users.Commands.EditPersonalData;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DBetter.Api;

public static class UserModule
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPatch("users/{id}", async (
                ClaimsPrincipal user,
                IMediator mediator,
                string id,
                EditPersonalDataParameters parameters) => 
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim is null || userIdClaim.Value != id)
                {
                    return Results.Unauthorized();
                }
                
                var command = Builder<EditPersonalDataCommand>
                    .BindParameters(parameters)
                    .MapParameter(p => p.Id, id)
                    .BuildUsing<EditPersonalDataRequestBuilder>();

                if (command.HasFailed) return Results.BadRequest();

                var result = await mediator.Send(command.Value);
                if (result.HasFailed) return Results.BadRequest();

                return Results.Ok(result.Value);
            })
                .RequireAuthorization()
                .WithName("EditPersonalData")
                .WithOpenApi();
    }
}
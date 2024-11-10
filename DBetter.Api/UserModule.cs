using System.Security.Claims;
using CleanDomainValidation.Application;
using DBetter.Application.Errors;
using DBetter.Application.Users.Commands.AddDiscount;
using DBetter.Application.Users.Commands.EditPersonalData;
using DBetter.Application.Users.Queries.GetMyPassengers;
using DBetter.Contracts.Users.Commands.AddDiscount;
using DBetter.Contracts.Users.Commands.EditPersonalData;
using DBetter.Contracts.Users.Queries.GetMyPassengers;
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
        
        app.MapPost("users/{id}/discounts", async (
                ClaimsPrincipal user,
                IMediator mediator,
                string id,
                AddDiscountParameters parameters) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null || userIdClaim.Value != id)
            {
                return Results.Unauthorized();
            }
                
            var command = Builder<AddDiscountCommand>
                .BindParameters(parameters)
                .MapParameter(p => p.UserId, id)
                .BuildUsing<AddDiscountRequestBuilder>();

            if (command.HasFailed) return Results.BadRequest();

            var result = await mediator.Send(command.Value);
            if (result.HasFailed) return Results.BadRequest();

            return Results.Ok();
        })
            .RequireAuthorization()
            .WithName("AddDiscount")
            .WithOpenApi();
        
        app.MapGet("users/{id}/passengers", async (
            ClaimsPrincipal user,
            IMediator mediator,
            string id) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null || userIdClaim.Value != id)
            {
                return Results.Unauthorized();
            }
                
            var query = Builder<GetMyPassengersQuery>
                .BindParameters(new GetMyPassengersParameters())
                .MapParameter(p => p.UserId, id)
                .BuildUsing<GetMyPassengersRequestBuilder>();

            if (query.HasFailed) return Results.BadRequest();

            var result = await mediator.Send(query.Value);
            if (result.HasFailed) return Results.BadRequest();

            return Results.Ok(result.Value);
        })
            .RequireAuthorization()
            .WithName("GetMyPassengers")
            .WithOpenApi();
    }
}
using System.Security.Claims;
using CleanDomainValidation.Application;
using CleanMediator;
using DBetter.Application.Users.Commands.AddDiscount;
using DBetter.Application.Users.Commands.EditPersonalData;
using DBetter.Application.Users.Queries.GetMyPassengers;
using DBetter.Contracts.Users.Commands.AddDiscount;
using DBetter.Contracts.Users.Commands.EditPersonalData;
using DBetter.Contracts.Users.Queries.GetMyPassengers;

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
                    .WithName("Users.EditPersonalData")
                    .BindParameters(parameters)
                    .MapParameter(p => p.Id, id)
                    .BuildUsing<EditPersonalDataRequestBuilder>();

                return await mediator.HandleCommandAsync(command, (EditPersonalDataResult user) =>
                {
                    return Results.Ok(user);
                });
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
                .WithName("Users.AddDiscount")
                .BindParameters(parameters)
                .MapParameter(p => p.UserId, id)
                .BuildUsing<AddDiscountRequestBuilder>();

            return await mediator.HandleCommandAsync(command, () =>
            {
                return Results.Ok();
            });
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
                .WithName("Users.GetMyPassengers")
                .BindParameters(new GetMyPassengersParameters())
                .MapParameter(p => p.UserId, id)
                .BuildUsing<GetMyPassengersRequestBuilder>();

            return await mediator.HandleQueryAsync(query, (MyPassengersResult passengersResult) =>
            {
                return Results.Ok(passengersResult);
            });
        })
            .RequireAuthorization()
            .WithName("GetMyPassengers")
            .WithOpenApi();
    }
}
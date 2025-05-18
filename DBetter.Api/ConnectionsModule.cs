using System.Security.Claims;
using CleanDomainValidation.Application;
using DBetter.Application.Connections.Commands.CreateRequest;
using DBetter.Application.Connections.Queries.GetSuggestions;
using DBetter.Application.Connections.Queries.GetWithIncreasedTransferTime;
using DBetter.Contracts.Connections.Queries.CreateRequest.Parameters;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Parameters;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Contracts.Connections.Queries.GetWithIncreasedTransferTime;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DBetter.Api;

public static class ConnectionsModule
{
    public static void AddConnectionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("connections/requests", async (
                ClaimsPrincipal user,
                IMediator mediator,
                ConnectionRequestParameters parameters) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                var ownerId = userIdClaim?.Value;
                
                var command = Builder<CreateConnectionRequestCommand>
                    .WithName("Connections.Requests.Create")
                    .BindParameters(parameters)
                    .MapParameter(r => r.OwnerId, ownerId)
                    .BuildUsing<CreateConnectionRequestCommandBuilder>();

                return await mediator.HandleCommandAsync(command, (ConnectionSuggestionsDto result) =>
                {
                    return Results.Ok(result);
                });
            })
            .WithName("CreateRequest")
            .Produces<ConnectionSuggestionsDto>()
            .WithOpenApi();
        
        app.MapGet("connections/requests/{id}/suggestions", async (
            IMediator mediator,
            string id, 
            [FromQuery(Name = "page")] string? page) =>
            {
                var command = Builder<GetConnectionSuggestionsQuery>
                    .WithName("Connections.Suggestions")
                    .BindParameters(new GetSuggestionsParameters())
                    .MapParameter(r => r.Id, id)
                    .MapParameter(r => r.Page, page)
                    .BuildUsing<GetConnectionSuggestionsQueryBuilder>();

                return await mediator.HandleCommandAsync(command, (ConnectionSuggestionsDto result) =>
                {
                    return Results.Ok(result);
                });
            })
        .WithName("GetSuggestions")
        .Produces<ConnectionSuggestionsDto>()
        .WithOpenApi();
        
        app.MapPost("connections/{id}/increasetransfertime", async (
                IMediator mediator,
                string id,
                GetWithIncreasedTransferTimeParameters parameters) =>
        {
            var command = Builder<GetWithIncreasedTransferTimeQuery>
                .WithName("Connections.WithIncreasedTransferTime")
                .BindParameters(parameters)
                .MapParameter(r => r.Id, id)
                .BuildUsing<GetWithIncreasedTransferTimeQueryBuilder>();

            return await mediator.HandleCommandAsync(command, (ConnectionDto result) =>
            {
                return Results.Ok(result);
            });
        }).WithName("GetConnectionWithIncreasedTransferTime")
        .Produces<ConnectionDto>()
        .WithOpenApi();
    }
}
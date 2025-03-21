using System.Security.Claims;
using CleanDomainValidation.Application;
using DBetter.Application.Connections.Queries.GetSuggestions;
using DBetter.Application.Connections.Queries.GetWithIncreasedTransferTime;
using DBetter.Contracts.ConnectionRequests.Commands.Put;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Parameters;
using DBetter.Contracts.Connections.Queries.GetWithIncreasedTransferTime;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Infrastructure.Postgres;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Api;

public static class ConnectionsModule
{
    public static void AddConnectionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("connections/suggestions", async (
                ClaimsPrincipal user,
                IMediator mediator,
                [FromQuery(Name = "page")] string? page,
                ConnectionRequestParameters parameters) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                var ownerId = userIdClaim?.Value;
                
                var command = Builder<GetConnectionSuggestionsQuery>
                    .WithName("Connections.Suggestions")
                    .BindParameters(parameters)
                    .MapParameter(r => r.OwnerId, ownerId)
                    .MapParameter(r => r.Page, page)
                    .BuildUsing<GetConnectionSuggestionsQueryBuilder>();

                return await mediator.HandleCommandAsync(command, (List<Connection> result) =>
                {
                    return Results.Ok(result);
                });
            })
            .WithName("GetConnectionSuggestions")
            .Produces<List<Connection>>()
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

            return await mediator.HandleCommandAsync(command, (Connection result) =>
            {
                return Results.Ok(result);
            });
        }).WithName("GetConnectionWithIncreasedTransferTime")
        .Produces<Connection>()
        .WithOpenApi();
    }
}
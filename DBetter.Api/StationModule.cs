using CleanDomainValidation.Application;
using CleanMediator;
using CleanMediator.Commands;
using DBetter.Application.Stations.Events;
using DBetter.Application.Stations.Queries.Find;
using DBetter.Application.Stations.ScrapeDepartures;
using DBetter.Contracts.Stations.Queries.Find;
using DBetter.Domain.Stations.Events;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.OutboxPattern;
using DBetter.Infrastructure.Postgres;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Api;

public static class StationModule
{
    public static void AddStationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/stations", async (IMediator mediator, [FromQuery(Name = "query")] string? query) =>
            {
                var command = Builder<FindStationsQuery>
                    .WithName("Stations.Find")
                    .BindParameters(new FindStationParameters())
                    .MapParameter(p => p.Query, query)
                    .BuildUsing<FindStationsRequestBuilder>();

                return await mediator.HandleQueryAsync(command, (List<StationDto> user) =>
                {
                    return Results.Ok(user);
                });
            })
            .WithName("SearchStations")
            .WithOpenApi();

        app.MapGet("/stations/debug", async (IMediator mediator) =>
        {
            await mediator.ExecuteAsync(
                new ScrapeStationDeparturesCommand(StationId.Create("1da5ecec-ee8c-43b4-8d12-e7d7a7603af4").Value,
                    new DateOnly(2026, 3, 18)));
            return Results.Ok();
        });
    }
}
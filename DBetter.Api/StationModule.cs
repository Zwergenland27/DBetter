using CleanDomainValidation.Application;
using CleanMediator;
using DBetter.Application.Stations.Events;
using DBetter.Application.Stations.Queries.Find;
using DBetter.Contracts.Stations.Queries.Find;
using DBetter.Domain.Stations.Events;
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

        app.MapGet("/stations/debug", async (DBetterContext db) =>
        {
            var allStationIds = await db.Stations
                .Select(s => s.Id)
                .ToListAsync();

            var events = allStationIds.Select(id => OutboxMessage.FromEvent(new UnknownStationCreatedEvent(id)));
            db.OutboxMessages.AddRange(events);
            await db.SaveChangesAsync();
            return Results.Ok(allStationIds.Count);
        });
    }
}
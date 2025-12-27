using CleanDomainValidation.Application;
using CleanMediator;
using DBetter.Application.Stations.Queries.Find;
using DBetter.Contracts.Stations.Queries.Find;
using Microsoft.AspNetCore.Mvc;

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
    }
}
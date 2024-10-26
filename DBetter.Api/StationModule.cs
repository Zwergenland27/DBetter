using DBetter.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DBetter.Api;

public static class StationModule
{
    public static void AddStationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/stations", async (HttpClient http, [FromQuery(Name = "search")] string? search) =>
            {
                var stations = await http.GetFromJsonAsync<List<Station>>($"reiseloesung/orte?suchbegriff={search}");
                return Results.Ok(stations);
            })
            .WithName("SearchStations")
            .WithOpenApi();
    }
}
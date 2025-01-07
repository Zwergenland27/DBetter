using DBetter.Contracts.Journeys.Parameters;
using DBetter.Infrastructure.BahnApi.Journey;
using Microsoft.AspNetCore.Mvc;

namespace DBetter.Api;

public static class SearchModule
{
    public static void AddSearchEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/search", async (JourneyRepository repository, [FromQuery(Name = "page")] string? page, RequestParameters parameters) =>
            {
                var result = await repository.GetRoutes(parameters, page);
                if (result is null) return Results.StatusCode(500);
                return Results.Ok(result);
            })
        .WithName("Search")
        .WithOpenApi();
        
        app.MapPost("/connection", async (JourneyRepository repository, [FromQuery(Name = "transferIncreaseType")] string? transferIncreaseType, IncreaseTransferTimeRequestParameters parameters) =>
            {
                if(transferIncreaseType is null) return Results.StatusCode(500);
                var result = await repository.GetRoute(parameters, transferIncreaseType);
                if (result is null) return Results.StatusCode(500);
                return Results.Ok(result);
            })
            .WithName("Connection")
            .WithOpenApi();
        
        app.MapGet("/section", async (JourneyRepository repository, [FromQuery(Name = "journeyId")] string? journeyId) =>
        {
            var result = await repository.GetSection(journeyId);
            if (result is null) return Results.StatusCode(500);
            return Results.Ok(result);
        })
            .WithName("GetSection")
            .WithOpenApi();
    }
}
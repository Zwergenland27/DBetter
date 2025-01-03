using DBetter.Contracts.Journeys.Parameters;
using DBetter.Infrastructure.BahnApi.Journey;

namespace DBetter.Api;

public static class SearchModule
{
    public static void AddSearchEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/search", async (JourneyRepository repository, RequestParameters parameters) =>
            {
                var result = await repository.GetRoutes(parameters);
                return Results.Ok(result);
            })
        .WithName("Search")
        .WithOpenApi();
    }
}
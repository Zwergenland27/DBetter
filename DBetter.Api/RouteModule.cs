using CleanDomainValidation.Application;
using CleanMediator;
using DBetter.Application.Routes.Queries.Get;
using DBetter.Contracts.Routes.Queries.Get;
using Microsoft.AspNetCore.Mvc;

namespace DBetter.Api;

public static class RouteModule
{
    public static void AddRouteEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("trainRuns", async (
                IMediator mediator,
                [FromQuery(Name = "id")] string? id) =>
            {
                var query = Builder<GetRouteQuery>
                    .WithName("TrainRun.Get")
                    .BindParameters(new GetRouteParameters())
                    .MapParameter(p => p.Id, id)
                    .BuildUsing<GetRouteQueryBuilder>();

                return await mediator.HandleCommandAsync(query, (RouteDto result) =>
                {
                    return Results.Ok(result);
                });
            })
        .WithName("GetTrainRuns")
        .WithOpenApi();
    }
}
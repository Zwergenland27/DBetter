using System.Numerics;
using CleanDomainValidation.Application;
using CleanMediator;
using CleanMediator.Commands;
using CleanMediator.Queries;
using DBetter.Application.TrainCompositions.Get;
using DBetter.Application.TrainRuns.Queries.Get;
using DBetter.Application.TrainRuns.Queries.GetRoute;
using DBetter.Contracts.TrainCompositions.Get;
using DBetter.Contracts.TrainRuns.Queries.Get;
using DBetter.Contracts.TrainRuns.Queries.Get.Results;
using DBetter.Contracts.TrainRuns.Queries.GetRoute;
using Microsoft.OpenApi;

namespace DBetter.Api;

public static class TrainRunModule
{
    public static void AddTrainRunEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("train-runs/{id}", async (
                IMediator mediator,
                HttpContext httpContext,
                string id) =>
            {
                var acceptHeader = httpContext.Request.Headers.Accept.ToString();
                if (acceptHeader is "application/dbetter.route-only+json")
                {
                    var routeOnlyQuery = Builder<GetRouteQuery>
                        .WithName("TrainRun.GetRoute")
                        .BindParameters(new GetRouteParameters())
                        .MapParameter(p => p.Id, id)
                        .BuildUsing<GetRouteQueryBuilder>();

                    return await mediator.HandleQueryAsync(routeOnlyQuery, (RouteResponse result) =>
                    {
                        return Results.Ok(result);
                    });
                }

                var query = Builder<GetTrainRunQuery>
                    .WithName("TrainRun.Get")
                    .BindParameters(new GetTrainRunParameters())
                    .MapParameter(p => p.Id, id)
                    .BuildUsing<GetTrainRunQueryBuilder>();

                return await mediator.HandleCommandAsync(query, (TrainRunResponse result) =>
                {
                    return Results.Ok(result);
                });
            })
            .WithName("GetTrainRun")
            .Produces<TrainRunResponse>(200, "application/json")
            .Produces<RouteResponse>(200, "application/dbetter.route-only+json");
        
        app.MapGet("train-runs/{id}/trainComposition", async (
                IMediator mediator,
                string id) =>
            {
                var query = Builder<GetTrainCompositionQuery>
                    .WithName("TrainRun.GetComposition")
                    .BindParameters(new GetTrainCompositionDto())
                    .MapParameter(p => p.TrainRunId, id)
                    .BuildUsing<GetTrainCompositionQueryBuilder>();

                return await mediator.HandleQueryAsync(query, (GetTrainCompositionResultDto result) =>
                {
                    return Results.Ok(result);
                });
            })
            .WithName("GetTrainComposition")
            .Produces<GetTrainCompositionResultDto>()
            .WithOpenApi();
    }
}
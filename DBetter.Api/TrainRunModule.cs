using CleanDomainValidation.Application;
using CleanMediator;
using DBetter.Application.TrainCompositions.Get;
using DBetter.Application.TrainRuns.Queries.Get;
using DBetter.Contracts.TrainCompositions.Get;
using DBetter.Contracts.TrainRuns.Queries.Get;
using DBetter.Contracts.TrainRuns.Queries.Get.Results;

namespace DBetter.Api;

public static class TrainRunModule
{
    public static void AddTrainRunEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("train-runs/{id}", async (
                IMediator mediator,
                string id) =>
            {
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
        .Produces<TrainRunResponse>()
        .WithOpenApi();
        
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
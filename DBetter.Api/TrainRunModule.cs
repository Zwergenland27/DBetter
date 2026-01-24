using CleanDomainValidation.Application;
using CleanMediator;
using DBetter.Application.TrainRuns.Queries.Get;
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
    }
}
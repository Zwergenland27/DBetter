using CleanDomainValidation.Application;
using DBetter.Application.TrainRuns.Queries.Get;
using DBetter.Contracts.TrainRuns.Queries.Get;
using DBetter.Domain.Routes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Route = DBetter.Domain.Routes.Route;

namespace DBetter.Api;

public static class TrainRunModule
{
    public static void AddTrainRunEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("trainRuns", async (
                IMediator mediator,
                [FromQuery(Name = "id")] string? id) =>
            {
                var query = Builder<GetTrainRunQuery>
                    .WithName("TrainRun.Get")
                    .BindParameters(new GetTrainRunParameters())
                    .MapParameter(p => p.Id, id)
                    .BuildUsing<GetTrainRunQueryBuilder>();

                return await mediator.HandleCommandAsync(query, (Route result) =>
                {
                    return Results.Ok(result);
                });
            })
        .WithName("GetTrainRuns")
        .WithOpenApi();
    }
}
using CleanDomainValidation.Application;
using CleanMediator;
using DBetter.Application.Routes.Queries.Get;
using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Contracts.Routes.Queries.Get.Results;
using Microsoft.AspNetCore.Mvc;

namespace DBetter.Api;

public static class RouteModule
{
    public static void AddRouteEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("routes/{id}", async (
                IMediator mediator,
                string id) =>
            {
                var query = Builder<GetRouteQuery>
                    .WithName("TrainRun.Get")
                    .BindParameters(new GetRouteParameters())
                    .MapParameter(p => p.Id, id)
                    .BuildUsing<GetRouteQueryBuilder>();

                return await mediator.HandleCommandAsync(query, (RouteResponse result) =>
                {
                    return Results.Ok(result);
                });
            })
        .WithName("GetRoute")
        .Produces<RouteResponse>()
        .WithOpenApi();
    }
}
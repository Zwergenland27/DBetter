using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using CleanMediator.Queries;
using DBetter.Contracts.TrainRuns.Queries.GetRoute;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.TrainRuns.Queries.GetRoute;

public class GetRouteQueryBuilder : IRequestBuilder<GetRouteParameters, GetRouteQuery>
{
    public ValidatedRequiredProperty<GetRouteQuery> Configure(RequiredPropertyBuilder<GetRouteParameters, GetRouteQuery> builder)
    {
        var id = builder.ClassProperty(r => r.TrainRunId)
            .Required()
            .Map(p => p.Id, TrainRunId.Create);
        
        return builder.Build(() => new GetRouteQuery(id));
    }
}

public record GetRouteQuery(TrainRunId TrainRunId): IQuery<RouteResponse>;
using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.TrainRuns.Queries.Get;
using DBetter.Domain.Routes;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Application.TrainRuns.Queries.Get;

public class GetTrainRunQueryBuilder : IRequestBuilder<GetTrainRunParameters, GetTrainRunQuery>
{
    public ValidatedRequiredProperty<GetTrainRunQuery> Configure(RequiredPropertyBuilder<GetTrainRunParameters, GetTrainRunQuery> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required()
            .Map(p => p.Id, RouteId.Create);
        
        return builder.Build(() => new GetTrainRunQuery(id));
    }
}

public record GetTrainRunQuery(RouteId Id) : ICommand<Route>;
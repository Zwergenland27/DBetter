using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.TrainRuns.Queries.Get;
using DBetter.Domain.TrainRun;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Application.TrainRuns.Queries.Get;

public class GetTrainRunQueryBuilder : IRequestBuilder<GetTrainRunParameters, GetTrainRunQuery>
{
    public ValidatedRequiredProperty<GetTrainRunQuery> Configure(RequiredPropertyBuilder<GetTrainRunParameters, GetTrainRunQuery> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required()
            .Map(p => p.Id, TrainRunId.Create);
        
        return builder.Build(() => new GetTrainRunQuery(id));
    }
}

public record GetTrainRunQuery(TrainRunId Id) : ICommand<TrainRun>;
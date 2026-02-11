using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Contracts.TrainCompositions.Get;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.TrainCompositions.Get;

public class GetTrainCompositionQueryBuilder : IRequestBuilder<GetTrainCompositionDto, GetTrainCompositionQuery>
{
    public ValidatedRequiredProperty<GetTrainCompositionQuery> Configure(RequiredPropertyBuilder<GetTrainCompositionDto, GetTrainCompositionQuery> builder)
    {
        var trainRunId = builder.ClassProperty(r => r.TrainRunId)
            .Required()
            .Map(p => p.TrainRunId, TrainRunId.Create);
        
        return builder.Build(() => new GetTrainCompositionQuery(trainRunId));
    }
}
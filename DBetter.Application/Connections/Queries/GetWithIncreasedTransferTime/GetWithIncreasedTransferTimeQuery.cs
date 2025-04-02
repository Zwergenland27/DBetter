using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Contracts.Connections.Queries.GetWithIncreasedTransferTime;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using TravelTime = DBetter.Domain.Routes.ValueObjects.TravelTime;

namespace DBetter.Application.Connections.Queries.GetWithIncreasedTransferTime;

public class GetWithIncreasedTransferTimeQueryBuilder : IRequestBuilder<GetWithIncreasedTransferTimeParameters, GetWithIncreasedTransferTimeQuery>
{
    public ValidatedRequiredProperty<GetWithIncreasedTransferTimeQuery> Configure(RequiredPropertyBuilder<GetWithIncreasedTransferTimeParameters, GetWithIncreasedTransferTimeQuery> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required()
            .Map(p => p.Id, ConnectionId.Create);

        var fixedStartEvaNumber = builder.ClassProperty(r => r.FixedStartEvaNumber)
            .Required()
            .Map(p => p.FixedStartEvaNumber, EvaNumber.Create);

        var fixedStartTime = builder.ClassProperty(r => r.FixedStartTime)
            .Required()
            .Map(p => p.FixedStartTime, value => new TravelTime(value, null));
        
        var fixedEndEvaNumber = builder.ClassProperty(r => r.FixedEndEvaNumber)
            .Required()
            .Map(p => p.FixedEndEvaNumber, EvaNumber.Create);

        var fixedEndTime = builder.ClassProperty(r => r.FixedEndTime)
            .Required()
            .Map(p => p.FixedEndTime, value => new TravelTime(value, null));
        
        return builder.Build(() => new GetWithIncreasedTransferTimeQuery(
            id,
            fixedStartEvaNumber,
            fixedStartTime,
            fixedEndEvaNumber,
            fixedEndTime));
    }
}

public record GetWithIncreasedTransferTimeQuery(
    ConnectionId Id,
    EvaNumber FixedStartEvaNumber,
    TravelTime FixedStartTime,
    EvaNumber FixedEndEvaNumber,
    TravelTime FixedEndTime) : ICommand<ConnectionDto>;
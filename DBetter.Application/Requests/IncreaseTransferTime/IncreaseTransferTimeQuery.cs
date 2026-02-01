using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Connections.Dtos;
using DBetter.Application.Requests.Dtos;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Contracts.Requests.Queries.GetWithIncreasedTransferTime;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Requests.IncreaseTransferTime;

public class IncreaseTransferTimeQueryBuilder : IRequestBuilder<GetWithIncreasedTransferTimeDto, IncreaseTransferTimeQuery>
{
    public ValidatedRequiredProperty<IncreaseTransferTimeQuery> Configure(RequiredPropertyBuilder<GetWithIncreasedTransferTimeDto, IncreaseTransferTimeQuery> builder)
    {
        var userId = builder.ClassProperty(r => r.UserId)
            .Optional()
            .Map(p => p.UserId, UserId.Create);
        
        var connectionRequestId = builder.ClassProperty(r => r.RequestId)
            .Required()
            .Map(p => p.ConnectionRequestId, ConnectionRequestId.Create);
        
        var connectionId = builder.ClassProperty(r => r.ConnectionId)
            .Required()
            .Map(p => p.ConnectionId, ConnectionId.Create);

        var transferIndex = builder.ClassProperty(r => r.TransferIndex)
            .Required()
            .Map(p => p.TransferId, v => new TransferIndex(v));

        var mode = builder.EnumProperty(r => r.Mode)
            .Required()
            .Map(p => p.Mode, Error.Validation("Request.IncreaseTransferTime.IncreaseTransferTimeMode.Invalid", "The increase transfer time mode must be valid"));
       
        return builder.Build(() => new IncreaseTransferTimeQuery(userId, connectionRequestId, connectionId, transferIndex, mode));
    }
}

public record IncreaseTransferTimeQuery(UserId? UserId, ConnectionRequestId RequestId, ConnectionId ConnectionId, TransferIndex TransferIndex, IncreaseTransferTimeMode Mode) : IQuery<ConnectionResponse>;
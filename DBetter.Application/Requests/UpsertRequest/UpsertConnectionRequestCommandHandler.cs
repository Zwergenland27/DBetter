using CleanDomainValidation.Domain;
using CleanMediator.Commands;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;

namespace DBetter.Application.Requests.UpsertRequest;

public class UpsertConnectionRequestCommandHandler(
    IUnitOfWork unitOfWork,
    IConnectionSuggestionService suggestionService,
    IConnectionRequestRepository connectionRequestRepository) : CommandHandlerBase<UpsertConnectionRequestCommand, List<ConnectionResponse>>
{
    public override async Task<CanFail<List<ConnectionResponse>>> Handle(UpsertConnectionRequestCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var connectionRequest = request.Request;
        
        var suggestionsDto = await suggestionService.GetConnectionSuggestionsAsync(connectionRequest, SuggestionMode.Normal);

        var earlierRef = suggestionsDto.EarlierRef;
        var laterRef = suggestionsDto.LaterRef;
        
        if (earlierRef is not null && laterRef is not null)
        {
            connectionRequest.InitializeLaterReference(earlierRef,  laterRef);
        }
        
        connectionRequestRepository.Store(connectionRequest);
        
        await unitOfWork.CommitAsync(cancellationToken);
        return suggestionsDto.Connections;
    }
}
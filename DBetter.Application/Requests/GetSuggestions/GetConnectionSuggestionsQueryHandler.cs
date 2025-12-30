using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Errors;

namespace DBetter.Application.Requests.GetSuggestions;

public class GetConnectionSuggestionsQueryHandler(
    IUnitOfWork unitOfWork,
    IConnectionSuggestionService suggestionService,
    IConnectionRequestRepository connectionRequestRepository) : QueryHandlerBase<GetConnectionSuggestionsQuery, List<ConnectionResponse>>
{
    public override async Task<CanFail<List<ConnectionResponse>>> Handle(GetConnectionSuggestionsQuery request, CancellationToken cancellationToken)
    {
        //TODO: possible add token to verify that server and client earlierRef expectations are in sync
        await unitOfWork.BeginTransaction(cancellationToken);
        var connectionRequest = await connectionRequestRepository.GetById(request.Id);
        if (connectionRequest is null) return DomainErrors.ConnectionRequest.NotFound;

        if (connectionRequest.OwnerId is not null && request.UserId is null) return DomainErrors.ConnectionRequest.Unauthorized;
        
        if(connectionRequest.OwnerId != request.UserId) return DomainErrors.ConnectionRequest.Unauthorized;
        
        var suggestionsDto = await suggestionService.GetConnectionSuggestionsAsync(connectionRequest, request.SuggestionMode);

        var earlierRef = suggestionsDto.EarlierRef;
        var laterRef = suggestionsDto.LaterRef;
        
        if (request.SuggestionMode is SuggestionMode.Normal && earlierRef is not null && laterRef is not null)
        {
            connectionRequest.InitializeLaterReference(earlierRef, laterRef);
        }
        else if (request.SuggestionMode is SuggestionMode.Earlier && earlierRef is not null)
        {
            connectionRequest.EarlierUsed(earlierRef);
        }else if (request.SuggestionMode is SuggestionMode.Later && laterRef is not null)
        {
            connectionRequest.LaterUsed(laterRef);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
        return suggestionsDto.Connections;
    }
}
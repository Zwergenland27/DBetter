using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Commands;
using CleanMediator.Queries;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Application.Requests.UpsertRequest;

public class UpsertConnectionRequestCommandHandler(
    IMediator mediator,
    IUnitOfWork unitOfWork,
    IConnectionRequestRepository connectionRequestRepository) : CommandHandlerBase<UpsertConnectionRequestCommand, List<ConnectionResponse>>
{
    public override async Task<CanFail<List<ConnectionResponse>>> Handle(UpsertConnectionRequestCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        await connectionRequestRepository.StoreAsync(request.Request);
        await unitOfWork.CommitAsync(cancellationToken);
        
        return await mediator.RunAsync(new GetConnectionSuggestionsQuery(request.Request.Id, request.Request.OwnerId, SuggestionMode.Normal), cancellationToken: cancellationToken);
    }
}
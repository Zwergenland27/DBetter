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
        var existingRequest = await connectionRequestRepository.GetAsync(request.Id);
        if (existingRequest is null)
        {
            var newRequestResult = ConnectionRequest.Create(
                request.Id,
                request.OwnerId,
                request.DepartureTime,
                request.ArrivalTime,
                request.Passengers,
                request.ComfortClass,
                request.Route);
            if (newRequestResult.HasFailed) return newRequestResult.Errors;
            
            connectionRequestRepository.Add(newRequestResult.Value);
        }
        else
        {
            existingRequest.Update(
                request.DepartureTime,
                request.ArrivalTime,
                request.Passengers,
                request.ComfortClass,
                request.Route);
        }
        await unitOfWork.CommitAsync(cancellationToken);
        
        return await mediator.RunAsync(new GetConnectionSuggestionsQuery(request.Id, request.OwnerId, SuggestionMode.Normal), cancellationToken: cancellationToken);
    }
}
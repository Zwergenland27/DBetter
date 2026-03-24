using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.Requests.Dtos;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Application.TrainCompositions;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Errors;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainRuns;

namespace DBetter.Application.Requests.IncreaseTransferTime;

public class IncreaseTransferTimeQueryHandler(
    IUnitOfWork unitOfWork,
    IExternalConnectionProvider externalConnectionProvider,
    IConnectionRepository connectionRepository,
    IConnectionRequestRepository connectionRequestRepository,
    ITrainCirculationRepository trainCirculationRepository,
    IRouteRepository routeRepository,
    ITrainRunRepository trainRunRepository,
    IPassengerInformationRepository passengerInformationRepository,
    IStationRepository stationRepository,
    ITrainCompositionQueryRepository trainCompositionRepository) : QueryHandlerBase<IncreaseTransferTimeQuery, ConnectionResponse>
{
    public override async Task<CanFail<ConnectionResponse>> Handle(IncreaseTransferTimeQuery query, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        
        var connectionRequest = await connectionRequestRepository.GetAsync(query.RequestId);
        if (connectionRequest is null) return DomainErrors.ConnectionRequest.NotFound;

        if (connectionRequest.OwnerId is not null && query.UserId is null) return DomainErrors.ConnectionRequest.Unauthorized;
        
        if(connectionRequest.OwnerId != query.UserId) return DomainErrors.ConnectionRequest.Unauthorized;
        
        if (!connectionRequest.SuggestedConnectionIds.Contains(query.ConnectionId))
            return DomainErrors.ConnectionRequest.ConnectionResults.NotSuggested;
        
        var connection = await connectionRepository.GetAsync(query.ConnectionId);
        if (connection is null) return DomainErrors.Connection.NotFound(query.ConnectionId);

        var transfer = connection.Transfers.FirstOrDefault(t => t.Id == query.TransferIndex);
        if (transfer is null) return DomainErrors.Connection.Transfer.Index.NotFound;

        var requestedStationIds = connectionRequest.PlannedRoute.GetRequestedStationIds();
        requestedStationIds.AddRange(connection.GetRequestedStationIds());
        
        var requestedStations = await stationRepository.GetManyAsync(requestedStationIds);
        
        var suggestionRequestFactory = new IncreaseTransferTimeRequestFactory(connectionRequest, connection, requestedStations);
        
        var foundConnection = await externalConnectionProvider.GetWithIncreasedTransferTime(suggestionRequestFactory.Build(transfer, query.Mode));
        if (foundConnection is null) return DomainErrors.ConnectionRequest.ConnectionResults.NothingFound;
        
        var extractor = new ConnectionExtractor(
                trainCirculationRepository,
                trainRunRepository,
                routeRepository,
                stationRepository,
                passengerInformationRepository,
                trainCompositionRepository)
            .ForConnections([foundConnection]);
        
        await extractor.Extract();
        var result = extractor.ExtractMissingInformation(connectionRequest.PlannedRoute);
        
        passengerInformationRepository.AddRange(result.PassengerInformationToCreate);
        connectionRepository.AddRange(result.FoundConnections);
        routeRepository.AddRange(result.RoutesToCreate);
        stationRepository.AddRange(result.StationsToCreate);
        
        connectionRequest.AddSuggestedConnections(result.FoundConnections);
        
        await unitOfWork.CommitAsync(cancellationToken);

        return extractor.ToResponses()[0];
    }
}
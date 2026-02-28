using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Abstractions.Persistence;
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

namespace DBetter.Application.Requests.GetSuggestions;

public class GetConnectionSuggestionsQueryHandler(
    IUnitOfWork unitOfWork,
    IExternalConnectionProvider externalConnectionProvider,
    IConnectionRequestRepository connectionRequestRepository,
    IConnectionRepository connectionRepository,
    IStationRepository stationRepository,
    ITrainRunRepository trainRunRepository,
    IRouteRepository routeRepository,
    ITrainCirculationRepository trainCirculationRepository,
    IPassengerInformationRepository passengerInformationRepository,
    ITrainCompositionQueryRepository trainCompositionRepository) : QueryHandlerBase<GetConnectionSuggestionsQuery, List<ConnectionResponse>>
{
    public override async Task<CanFail<List<ConnectionResponse>>> Handle(GetConnectionSuggestionsQuery request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var connectionRequest = await connectionRequestRepository.GetAsync(request.Id);
        if (connectionRequest is null) return DomainErrors.ConnectionRequest.NotFound;

        if (connectionRequest.OwnerId is not null && request.UserId is null) return DomainErrors.ConnectionRequest.Unauthorized;
        
        if(connectionRequest.OwnerId != request.UserId) return DomainErrors.ConnectionRequest.Unauthorized;

        var requestedStations = await stationRepository.GetManyAsync(connectionRequest.PlannedRoute.GetRequestedStationIds());
        
        var suggestionRequestFactory = new SuggestionRequestFactory(connectionRequest, requestedStations);
        
        var suggestionsDto = await externalConnectionProvider.GetSuggestions(suggestionRequestFactory.Build(request.SuggestionMode));
        _ = connectionRequest.UpdateReferences(request.SuggestionMode, suggestionsDto.EarlierRef, suggestionsDto.LaterRef);

        var extractor = new ConnectionExtractor(
                trainCirculationRepository,
                trainRunRepository,
                routeRepository,
                stationRepository,
                passengerInformationRepository,
                trainCompositionRepository)
            .ForConnections(suggestionsDto.Connections);
        
        await extractor.Extract();
        var result = extractor.ExtractMissingInformation(connectionRequest.PlannedRoute);
        
        passengerInformationRepository.AddRange(result.PassengerInformationToCreate);
        trainCirculationRepository.AddRange(result.TrainCirculationsToCreate);
        connectionRepository.AddRange(result.FoundConnections);
        trainRunRepository.AddRange(result.TrainRunsToCreate);
        routeRepository.AddRange(result.RoutesToCreate);
        stationRepository.AddRange(result.StationsToCreate);
        
        connectionRequest.AddSuggestedConnections(result.FoundConnections);
        
        await unitOfWork.CommitAsync(cancellationToken);

        return extractor.ToResponses();
    }
}
using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.Snapshots;
using DBetter.Domain.Errors;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations;
using Route = DBetter.Domain.Routes.Route;

namespace DBetter.Application.Requests.GetSuggestions;

public class GetConnectionSuggestionsQueryHandler(
    IUnitOfWork unitOfWork,
    IExternalConnectionProvider externalConnectionProvider,
    IConnectionRequestRepository connectionRequestRepository,
    IConnectionRepository connectionRepository,
    IStationRepository stationRepository,
    IRouteRepository routeRepository) : QueryHandlerBase<GetConnectionSuggestionsQuery, List<ConnectionResponse>>
{
    private List<Route> _existingRoutes = [];
    private List<Route> _routesToCreate = [];
    private List<Station> _existingStations = [];
    private List<Station> _stationsToCreate = [];
    
    public override async Task<CanFail<List<ConnectionResponse>>> Handle(GetConnectionSuggestionsQuery request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var connectionRequest = await connectionRequestRepository.GetById(request.Id);
        if (connectionRequest is null) return DomainErrors.ConnectionRequest.NotFound;

        if (connectionRequest.OwnerId is not null && request.UserId is null) return DomainErrors.ConnectionRequest.Unauthorized;
        
        if(connectionRequest.OwnerId != request.UserId) return DomainErrors.ConnectionRequest.Unauthorized;

        var requestedStations = await stationRepository.GetManyAsync(connectionRequest.Route.GetRequestedStationIds());
        
        var suggestionRequestFactory = new SuggestionRequestFactory(connectionRequest, requestedStations);
        
        var suggestionsDto = await externalConnectionProvider.GetSuggestions(suggestionRequestFactory.Build(request.SuggestionMode));
        _ = connectionRequest.UpdateReferences(request.SuggestionMode, suggestionsDto.EarlierRef, suggestionsDto.LaterRef);
        
        await ExtractConnectionsAndRoutesAndStations(suggestionsDto.Connections);
        var connections = new List<Connection>();

        foreach (var connection in suggestionsDto.Connections)
        {
            ExtractRouteInformation(connection);
            ExtractMissingStations(connection);
            connections.Add(Connection.CreateFromSnapshot(connection));
        }
        
        connectionRepository.AddRange(connections);
        routeRepository.AddRange(_routesToCreate);
        stationRepository.AddRange(_stationsToCreate);
        
        connectionRequest.AddSuggestedConnections(connections);
        
        await unitOfWork.CommitAsync(cancellationToken);

        var responseFactory = new ConnectionResponseFactory(connections, _existingRoutes, _existingStations);
            
        return suggestionsDto.Connections.Select(connection => responseFactory.MapToResponse(connection)).ToList();
    }
    private void ExtractRouteInformation(ConnectionSnapshot connectionSnapshot)
    {
        var routes = connectionSnapshot.Segments.OfType<TransportSegmentSnapshot>();
        foreach (var route in routes)
        {
            var existingRoute = _existingRoutes.FirstOrDefault(r => r.JourneyId == route.JourneyId);
            if (existingRoute is not null)
            {
                existingRoute.Update(route);
                continue;
            }
            
            var newRoute = Route.CreateFromSnapshot(route);
            _routesToCreate.Add(newRoute);
            _existingRoutes.Add(newRoute);
        }
    }

    private void ExtractMissingStations(ConnectionSnapshot connectionSnapshot)
    {
        var unknownStations = connectionSnapshot.GetUnknownStations(_existingStations);
        foreach (var station in unknownStations)
        {
            var newStation = Station.CreateFromSnapshot(station);
            _stationsToCreate.Add(newStation);
            _existingStations.Add(newStation);
        }
    }

    private async Task ExtractConnectionsAndRoutesAndStations(List<ConnectionSnapshot> connectionSnapshots)
    {
        var journeyIds = connectionSnapshots
            .SelectMany(cs => cs.JourneyIds)
            .ToList();
        
        var evaNumbers = connectionSnapshots
            .SelectMany(cs => cs.StationEvaNumbers)
            .ToList();
        
        evaNumbers.AddRange(journeyIds.SelectMany(jid => new []
        {
            jid.OriginEvaNumber,
            jid.DestinationEvaNumber
        }));
        _existingRoutes = await routeRepository.GetManyAsync(journeyIds);
        _existingStations = await stationRepository.GetManyAsync(evaNumbers.Distinct());
    }
}
using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.Requests.Dtos;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Errors;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.Snapshots;

namespace DBetter.Application.Requests.GetSuggestions;

public class GetConnectionSuggestionsQueryHandler(
    IUnitOfWork unitOfWork,
    IExternalConnectionProvider externalConnectionProvider,
    IConnectionRequestRepository connectionRequestRepository,
    IConnectionRepository connectionRepository,
    IStationRepository stationRepository,
    ITrainRunRepository trainRunRepository,
    IPassengerInformationRepository passengerInformationRepository) : QueryHandlerBase<GetConnectionSuggestionsQuery, List<ConnectionResponse>>
{
    private List<TrainRun> _existingTrainRuns = [];
    private List<TrainRun> _trainRunsToCreate = [];
    private List<Station> _existingStations = [];
    private List<Station> _stationsToCreate = [];
    private List<PassengerInformation> _existingPassengerInformation = [];
    private List<PassengerInformation> _passengerInformationToCreate = [];
    
    public override async Task<CanFail<List<ConnectionResponse>>> Handle(GetConnectionSuggestionsQuery request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var connectionRequest = await connectionRequestRepository.GetAsync(request.Id);
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
            ExtractPassengerInformation(connection);
            ExtractRouteInformation(connection);
            ExtractMissingStations(connection);

            var firstStop = connection.Segments.OfType<TransportSegmentDto>().First().Stops.First();
            connections.Add(Connection.Create(connection.ContextId, DateOnly.FromDateTime(firstStop.DepartureTime!.Planned)));
        }

        foreach (var r in _existingTrainRuns)
        {
            Console.WriteLine(r.JourneyId.Value);
        }
        
        connectionRepository.AddRange(connections);
        trainRunRepository.AddRange(_trainRunsToCreate);
        stationRepository.AddRange(_stationsToCreate);
        passengerInformationRepository.AddRange(_passengerInformationToCreate);
        
        connectionRequest.AddSuggestedConnections(connections);
        
        await unitOfWork.CommitAsync(cancellationToken);

        var responseFactory = new ConnectionResponseFactory(connections, _existingTrainRuns, _existingStations, _existingPassengerInformation);
            
        return suggestionsDto.Connections.Select(connection => responseFactory.MapToResponse(connection)).ToList();
    }
    
    private void ExtractPassengerInformation(ConnectionDto connectionDto)
    {
        var passengerInformation = connectionDto.Segments
            .OfType<TransportSegmentDto>()
            .SelectMany(ts => ts.PassengerInformation)
            .Distinct();

        foreach (var pim in passengerInformation)
        {
            var existingMessage = _existingPassengerInformation.FirstOrDefault(im => im.Text == pim.OriginalText);
            if (existingMessage is not null) continue;
            
            var newPassengerInformation = PassengerInformation.FoundNew(pim.OriginalText, pim.Priority);
            
            _passengerInformationToCreate.Add(newPassengerInformation);
            _existingPassengerInformation.Add(newPassengerInformation);
        }
    }
    private void ExtractRouteInformation(ConnectionDto connectionDto)
    {
        var routes = connectionDto.Segments.OfType<TransportSegmentDto>();
        foreach (var route in routes)
        {
            var passengerInformationSnapshots = route.PassengerInformation
                .Select(pimDto =>
                {
                    var pim = _existingPassengerInformation.First(im => im.Text == pimDto.OriginalText);
                    return new TrainRunPassengerInformationSnapshot(pim.Id, pimDto.FromStopIndex, pimDto.ToStopIndex);
                }).ToList();
            
            var existingRoute = _existingTrainRuns.FirstOrDefault(r => r.JourneyId == route.JourneyId);
            if (existingRoute is not null)
            {
                existingRoute.Update(route.BikeCarriage);
                existingRoute.Update(route.Catering);
                existingRoute.ReconcilePassengerInformation(passengerInformationSnapshots);
                continue;
            }
            
            var newRoute = TrainRun.Create(
                route.JourneyId,
                passengerInformationSnapshots,
                route.Composition.First(),
                route.Catering,
                route.BikeCarriage);
            
            _trainRunsToCreate.Add(newRoute);
            _existingTrainRuns.Add(newRoute);
        }
    }

    private void ExtractMissingStations(ConnectionDto connectionDto)
    {
        var unknownStations = connectionDto.GetUnknownStations(_existingStations);
        foreach (var station in unknownStations)
        {
            if (_existingStations.Any(existingStation => existingStation.EvaNumber == station.EvaNumber)) continue;
            var newStation = Station.Create(station.EvaNumber, station.Name, station.InfoId);
            _stationsToCreate.Add(newStation);
            _existingStations.Add(newStation);
        }
    }

    private async Task ExtractConnectionsAndRoutesAndStations(List<ConnectionDto> connectionSnapshots)
    {
        var journeyIds = connectionSnapshots
            .SelectMany(cs => cs.JourneyIds)
            .ToList();
        
        var evaNumbers = connectionSnapshots
            .SelectMany(cs => cs.StationEvaNumbers)
            .ToList();
        
        var passengerInformationTexts = connectionSnapshots
            .SelectMany(cs => cs.PassengerInformation)
            .Select(cs => cs.OriginalText)
            .ToList();
        
        evaNumbers.AddRange(journeyIds.SelectMany(jid => new []
        {
            jid.OriginEvaNumber,
            jid.DestinationEvaNumber
        }));
        
        _existingTrainRuns = await trainRunRepository.GetManyAsync(journeyIds.Distinct());
        _existingStations = await stationRepository.GetManyAsync(evaNumbers.Distinct());
        
        _existingPassengerInformation = await passengerInformationRepository.FindManyAsync(passengerInformationTexts);
        _existingPassengerInformation.AddRange(await passengerInformationRepository.GetManyAsync(_existingTrainRuns
            .SelectMany(r => r.PassengerInformation)
            .Select(pim => pim.InformationId)));
        _existingPassengerInformation = _existingPassengerInformation.Distinct().ToList();
    }
}
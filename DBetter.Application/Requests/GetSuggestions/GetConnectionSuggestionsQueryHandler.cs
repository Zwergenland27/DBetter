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
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
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
    ITrainCirculationRepository trainCirculationRepository,
    IPassengerInformationRepository passengerInformationRepository) : QueryHandlerBase<GetConnectionSuggestionsQuery, List<ConnectionResponse>>
{
    private List<TrainCirculation> _existingTrainCirculations = [];
    private List<TrainCirculation> _trainCirculationsToCreate = [];
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
        
        await ExtractExistingAggregates(suggestionsDto.Connections);
        var connections = new List<Connection>();

        foreach (var connection in suggestionsDto.Connections)
        {
            ExtractPassengerInformation(connection);
            ExtractTrainCirculations(connection);
            ExtractTrainRuns(connection);
            ExtractMissingStations(connection);

            var firstStop = connection.Segments.OfType<TransportSegmentDto>().First().Stops.First();
            connections.Add(Connection.Create(connection.ContextId, DateOnly.FromDateTime(firstStop.DepartureTime!.Planned)));
        }
        passengerInformationRepository.AddRange(_passengerInformationToCreate);
        trainCirculationRepository.AddRange(_trainCirculationsToCreate);
        connectionRepository.AddRange(connections);
        trainRunRepository.AddRange(_trainRunsToCreate);
        stationRepository.AddRange(_stationsToCreate);
        
        connectionRequest.AddSuggestedConnections(connections);
        
        await unitOfWork.CommitAsync(cancellationToken);

        var responseFactory = new ConnectionResponseFactory(connections, _existingTrainCirculations, _existingTrainRuns, _existingStations, _existingPassengerInformation);
            
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

    private void ExtractTrainCirculations(ConnectionDto connectionDto)
    {
        var transportSegments = connectionDto.Segments.OfType<TransportSegmentDto>();
        foreach (var transportSegment in transportSegments)
        {
            var existingTrainCirculation =
                _existingTrainCirculations.FirstOrDefault(tc =>
                    tc.TrainId == transportSegment.JourneyId.TrainId);
            if (existingTrainCirculation is not null)
            {
                continue;
            }

            var newTrainCirculation =
                TrainCirculation.Create(transportSegment.JourneyId, transportSegment.Composition.First());
            _existingTrainCirculations.Add(newTrainCirculation);
            _trainCirculationsToCreate.Add(newTrainCirculation);
        }
    }
    private void ExtractTrainRuns(ConnectionDto connectionDto)
    {
        var transportSegments = connectionDto.Segments.OfType<TransportSegmentDto>();
        foreach (var transportSegment in transportSegments)
        {
            var journeyId = transportSegment.JourneyId;
            var trainId = journeyId.TrainId;
            var operatingDay = journeyId.OperatingDay;
            var timeTablePeriod = TimeTablePeriod.FromOperatingDay(operatingDay);
            
            var passengerInformationSnapshots = transportSegment.PassengerInformation
                .Select(pimDto =>
                {
                    var pim = _existingPassengerInformation.First(im => im.Text == pimDto.OriginalText);
                    return new TrainRunPassengerInformationSnapshot(pim.Id, pimDto.FromStopIndex, pimDto.ToStopIndex);
                }).ToList();
            
            var trainCirculation = _existingTrainCirculations.First(tc => tc.TrainId == trainId && tc.TimeTablePeriod == timeTablePeriod);
            
            var existingTrainRun = _existingTrainRuns.FirstOrDefault(r => r.CirculationId == trainCirculation.Id && r.OperatingDay == operatingDay);
            if (existingTrainRun is not null)
            {
                existingTrainRun.Update(transportSegment.BikeCarriage);
                existingTrainRun.Update(transportSegment.Catering);
                existingTrainRun.ReconcilePassengerInformation(passengerInformationSnapshots);
                continue;
            }

            var newTrainRun = TrainRunFactory.Create(
                trainCirculation,
                journeyId,
                passengerInformationSnapshots,
                transportSegment.BikeCarriage,
                transportSegment.Catering);
            
            _trainRunsToCreate.Add(newTrainRun);
            _existingTrainRuns.Add(newTrainRun);
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

    private async Task ExtractExistingAggregates(List<ConnectionDto> connectionSnapshots)
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

        _existingTrainCirculations = await trainCirculationRepository.GetManyAsync(journeyIds.Select(jid => jid.CompositeKey).Distinct());
        
        _existingTrainRuns = await trainRunRepository.GetManyAsync(journeyIds);
        _existingStations = await stationRepository.GetManyAsync(evaNumbers.Distinct());
        
        _existingPassengerInformation = await passengerInformationRepository.FindManyAsync(passengerInformationTexts);
        _existingPassengerInformation.AddRange(await passengerInformationRepository.GetManyAsync(_existingTrainRuns
            .SelectMany(r => r.PassengerInformation)
            .Select(pim => pim.InformationId)));
        _existingPassengerInformation = _existingPassengerInformation.Distinct().ToList();
    }
}
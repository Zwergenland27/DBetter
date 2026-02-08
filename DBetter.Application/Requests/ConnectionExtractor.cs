using DBetter.Application.Connections.Dtos;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Routes;
using DBetter.Domain.Routes.Snapshots;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.Snapshots;

namespace DBetter.Application.Requests;

public class ConnectionExtractor(
    ITrainCirculationRepository trainCirculationRepository,
    ITrainRunRepository trainRunRepository,
    IRouteRepository routeRepository,
    IStationRepository stationRepository,
    IPassengerInformationRepository passengerInformationRepository)
{
    
    public List<TrainCirculation>? ExistingTrainCirculations;
    public List<TrainCirculation>? TrainCirculationsToCreate;
    public List<TrainRun>? ExistingTrainRuns;
    public List<TrainRun>? TrainRunsToCreate;
    public List<Route>? ExistingRoutes;
    public List<Route>? RoutesToCreate;
    public List<Station>? ExistingStations;
    public List<Station>? StationsToCreate;
    public List<PassengerInformation>? ExistingPassengerInformation;
    public List<PassengerInformation>? PassengerInformationToCreate;

    public List<Connection>? FoundConnections;
    
    private List<BahnJourneyId>? _journeyIds;
    private List<ConnectionDto>? _connections;

    public ConnectionExtractor ForConnections(List<ConnectionDto> connections)
    {
        _connections = connections;
        return this;
    }
    
    public async Task Extract()
    {
        if (_connections is null)
            throw new InvalidOperationException("Cannot extract data before connections have been set");
        ExtractJourneyIds(_connections);
        await ExtractStations(_connections);
        await ExtractTrainCirculations();
        await ExtractTrainRuns();
        await ExtractRoutes();
        await ExtractPassengerInformation(_connections);
    }

    public ConnectionExtractorResult ExtractMissingInformation(PlannedRoute route)
    {
        if (_connections is null)
            throw new InvalidOperationException("Cannot extract data before connections have been set");
        
        foreach (var connection in _connections)
        {
            ExtractMissingPassengerInformation(connection);
            ExtractMissingTrainCirculations(connection);
            ExtractMissingStations(connection);
            ExtractMissingTrainRunsWithRoutes(connection);
            ExtractMissingConnection(connection, route);
        }

        if (TrainCirculationsToCreate is null)
            throw new InvalidOperationException("Train circulations have not been extracted");
        if (TrainRunsToCreate is null)
            throw new InvalidOperationException("Train runs have not been extracted");
        if( RoutesToCreate is null)
            throw new InvalidOperationException("Routes have not been extracted");
        if (PassengerInformationToCreate is null)
            throw new InvalidOperationException("Passenger information have not been extracted");
        if (StationsToCreate is null)
            throw new InvalidOperationException("Stations have not been extracted");
        if (FoundConnections is null)
            throw new InvalidOperationException("Connections have not been extracted");

        return new ConnectionExtractorResult(
            TrainCirculationsToCreate,
            TrainRunsToCreate,
            RoutesToCreate,
            PassengerInformationToCreate,
            StationsToCreate,
            FoundConnections);
    }

    public List<ConnectionResponse> ToResponses()
    {
        if (_connections is null)
            throw new InvalidOperationException("Cannot map to responses before connections have been set");
            
        if (ExistingTrainCirculations is null)
            throw new InvalidOperationException("Train circulations have not been extracted");
        if (ExistingTrainRuns is null)
            throw new InvalidOperationException("Train runs have not been extracted");
        if (ExistingPassengerInformation is null)
            throw new InvalidOperationException("Passenger information have not been extracted");
        if (ExistingStations is null)
            throw new InvalidOperationException("Stations have not been extracted");
        if (FoundConnections is null)
            throw new InvalidOperationException("Connections have not been extracted");
        
        var responseFactory = new ConnectionResponseFactory(
            FoundConnections,
            ExistingTrainCirculations,
            ExistingTrainRuns, 
            ExistingStations, 
            ExistingPassengerInformation);
            
        return _connections.Select(connection => responseFactory.MapToResponse(connection)).ToList();
    }

    private void ExtractJourneyIds(List<ConnectionDto> connections)
    {
        _journeyIds = connections
            .SelectMany(c => c.JourneyIds)
            .Distinct()
            .ToList();
    }

    private async Task ExtractStations(List<ConnectionDto> connections)
    {
        if (_journeyIds is null)
            throw new InvalidOperationException("Cannot extract stations before journeyIds have been extracted.");
        
        var evaNumbers = connections
            .SelectMany(cs => cs.StationEvaNumbers)
            .Distinct()
            .ToList();
        
        evaNumbers.AddRange(_journeyIds.SelectMany(jid => new []
        {
            jid.OriginEvaNumber,
            jid.DestinationEvaNumber
        }));
        
        ExistingStations = await stationRepository.GetManyAsync(evaNumbers);
    }

    private async Task ExtractTrainCirculations()
    {
        if (_journeyIds is null)
            throw new InvalidOperationException("Cannot extract train circulations before journeyIds have been extracted.");
        ExistingTrainCirculations = await trainCirculationRepository.GetManyAsync(_journeyIds.Select(jid => jid.TimeTableCompositeIdentifier).Distinct());
    }

    private async Task ExtractTrainRuns()
    {
        if (_journeyIds is null)
            throw new InvalidOperationException("Cannot extract train runs before journeyIds have been extracted.");
        
        ExistingTrainRuns = await trainRunRepository.GetManyAsync(_journeyIds.Select(jid => jid.TrainRunCompositeIdentifier).Distinct());
    }

    private async Task ExtractRoutes()
    {
        if(ExistingTrainRuns is null)
            throw new InvalidOperationException("Cannot extract routes before trainRuns have been extracted.");

        ExistingRoutes = await routeRepository.GetManyAsync(ExistingTrainRuns.Select(tr => tr.Id));
    }

    private async Task ExtractPassengerInformation(List<ConnectionDto> connections)
    {
        if(ExistingTrainRuns is null) throw new InvalidOperationException("Cannot extract passenger information before train runs have been extracted.");
        
        var passengerInformationTexts = connections
            .SelectMany(cs => cs.PassengerInformation)
            .Select(cs => cs.OriginalText)
            .Distinct()
            .ToList();
        
        ExistingPassengerInformation = await passengerInformationRepository.FindManyAsync(passengerInformationTexts);
        ExistingPassengerInformation.AddRange(await passengerInformationRepository.GetManyAsync(ExistingTrainRuns
            .SelectMany(r => r.PassengerInformation)
            .Select(pim => pim.InformationId)));
        ExistingPassengerInformation = ExistingPassengerInformation.Distinct().ToList();
    }
    
    private void ExtractMissingPassengerInformation(ConnectionDto connectionDto)
    {
        if (ExistingPassengerInformation is null)
            throw new InvalidOperationException(
                "Cannot extract missing passenger information before existing passenger information have been extracted.");
        PassengerInformationToCreate ??= [];
        
        var passengerInformation = connectionDto.Segments
            .OfType<TransportSegmentDto>()
            .SelectMany(ts => ts.PassengerInformation)
            .Distinct();

        foreach (var pim in passengerInformation)
        {
            var existingMessage = ExistingPassengerInformation.FirstOrDefault(im => im.Text == pim.OriginalText);
            if (existingMessage is not null) continue;
            
            var newPassengerInformation = PassengerInformation.FoundNew(pim.OriginalText, pim.Priority);
            
            PassengerInformationToCreate.Add(newPassengerInformation);
            ExistingPassengerInformation.Add(newPassengerInformation);
        }
    }

    private void ExtractMissingTrainCirculations(ConnectionDto connectionDto)
    {
        if (ExistingTrainCirculations is null)
            throw new InvalidOperationException(
                "Cannot extract missing train circulations before existing train circulation information have been extracted.");
        TrainCirculationsToCreate ??= [];
        
        var transportSegments = connectionDto.Segments.OfType<TransportSegmentDto>();
        foreach (var transportSegment in transportSegments)
        {
            var compositeKey = transportSegment.JourneyId.TimeTableCompositeIdentifier;
            var existingTrainCirculation = ExistingTrainCirculations.FirstOrDefault(tc => tc.TrainId == compositeKey.TrainId && tc.TimeTablePeriod == compositeKey.TimeTablePeriod);
            if (existingTrainCirculation is not null)
            {
                continue;
            }

            var newTrainCirculation =
                TrainCirculation.Create(transportSegment.JourneyId, transportSegment.Composition.First());
            ExistingTrainCirculations.Add(newTrainCirculation);
            TrainCirculationsToCreate.Add(newTrainCirculation);
        }
    }
    private void ExtractMissingTrainRunsWithRoutes(ConnectionDto connectionDto)
    {
        if (ExistingTrainRuns is null)
            throw new InvalidOperationException(
                "Cannot extract missing train runs before existing train runs have been extracted.");
        if (ExistingTrainCirculations is null || TrainCirculationsToCreate is null)
            throw new InvalidOperationException(
                "Cannot extract missing train runs before existing AND missing train circulation have been extracted.");

        if (ExistingRoutes is null)
            throw new InvalidOperationException(
                "Cannot extract missing train runs before existing routes have been extracted.");

        if (ExistingPassengerInformation is null || PassengerInformationToCreate is null)
            throw new InvalidOperationException(
                "Cannot extract missing train runs before existing AND missing passenger information have been extracted.");
        
        TrainRunsToCreate ??= [];
        RoutesToCreate ??= [];
        
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
                    var pim = ExistingPassengerInformation.First(im => im.Text == pimDto.OriginalText);
                    return new TrainRunPassengerInformationSnapshot(pim.Id, pimDto.FromStopIndex, pimDto.ToStopIndex);
                }).ToList();
            
            var trainCirculation = ExistingTrainCirculations.First(tc => tc.TrainId == trainId && tc.TimeTablePeriod == timeTablePeriod);
            
            var existingTrainRun = ExistingTrainRuns.FirstOrDefault(r => r.CirculationId == trainCirculation.Id && r.OperatingDay == operatingDay);
            if (existingTrainRun is not null)
            {
                existingTrainRun.Update(transportSegment.BikeCarriage);
                existingTrainRun.Update(transportSegment.Catering);
                existingTrainRun.ReconcilePassengerInformation(passengerInformationSnapshots);
                
                var existingRoute = ExistingRoutes.First(r => r.TrainRunId == existingTrainRun.Id);
                existingRoute.UpdateFromRoute(ExtractStops(transportSegment.Stops));
                continue;
            }

            var newTrainRun = TrainRunFactory.Create(
                trainCirculation,
                journeyId,
                passengerInformationSnapshots,
                transportSegment.BikeCarriage,
                transportSegment.Catering);
            
            TrainRunsToCreate.Add(newTrainRun);
            ExistingTrainRuns.Add(newTrainRun);
            
            var newRoute = Route.CreateFromRoute(
                newTrainRun.Id,
                ExtractStops(transportSegment.Stops));
            
            RoutesToCreate.Add(newRoute);
            ExistingRoutes.Add(newRoute);
        }
    }

    private List<StopSnapshot> ExtractStops(List<StopDto> stopDtos)
    {
        if(ExistingStations is null)
            throw new InvalidOperationException("Cannot extract stops before existing stations have been extracted.");
        var stops =  new List<StopSnapshot>();

        foreach (var stop in stopDtos)
        {
            var station = ExistingStations.First(s => s.EvaNumber == stop.EvaNumber);
            stops.Add(new StopSnapshot(
                stop.TrainRunIndex,
                station.Id,
                stop.ArrivalTime,
                stop.DepartureTime,
                stop.Attributes));
        }
        
        return stops;
    }

    private void ExtractMissingStations(ConnectionDto connectionDto)
    {
        if (ExistingStations is null)
            throw new InvalidOperationException(
                "Cannot extract missing stations before existing stations have been extracted.");

        StationsToCreate ??= [];
        
        var unknownStations = connectionDto.GetUnknownStations(ExistingStations);
        foreach (var station in unknownStations)
        {
            if (ExistingStations.Any(existingStation => existingStation.EvaNumber == station.EvaNumber)) continue;
            var newStation = Station.Create(station.EvaNumber, station.Name, station.InfoId);
            StationsToCreate.Add(newStation);
            ExistingStations.Add(newStation);
        }
    }

    private void ExtractMissingConnection(ConnectionDto connectionDto, PlannedRoute route)
    {
        if (ExistingStations is null)
            throw new InvalidOperationException(
                "Cannot extract missing connection before existing stations have been extracted.");

        FoundConnections ??= [];
        
        var firstStop = connectionDto.Segments.OfType<TransportSegmentDto>().First().Stops.First();
        
        FoundConnections.Add(Connection.Create(
            connectionDto.ContextId,
            DateOnly.FromDateTime(firstStop.DepartureTime!.Planned),
            new SnapshotFactory(connectionDto.Segments, ExistingStations).ToSnapshot(),
            route));
    }
}
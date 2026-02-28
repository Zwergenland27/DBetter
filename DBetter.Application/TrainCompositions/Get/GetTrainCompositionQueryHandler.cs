using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Contracts.TrainCompositions.Get;
using DBetter.Domain.Errors;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainCompositions.Snapshots;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Domain.Vehicles;

namespace DBetter.Application.TrainCompositions.Get;

public class GetTrainCompositionQueryHandler(
    IUnitOfWork unitOfWork,
    IStationRepository stationRepository,
    ITrainCirculationRepository trainCirculationRepository,
    ITrainRunRepository trainRunRepository,
    IRouteRepository routeRepository,
    IVehicleRepository vehicleRepository,
    IExternalTrainCompositionProvider trainCompositionProvider,
    ITrainCompositionRepository trainCompositionRepository,
    ITrainCompositionPredictor predictor) : QueryHandlerBase<GetTrainCompositionQuery, GetTrainCompositionResultDto>
{
    private TrainRun? _trainRun;
    private TrainCirculation? _trainCirculation;
    private Route? _route;
    private List<Station>? _relevantStations;
    
    public override async Task<CanFail<GetTrainCompositionResultDto>> Handle(GetTrainCompositionQuery query, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);

        _trainRun = await trainRunRepository.GetAsync(query.TrainRunId);
        if (_trainRun is null) return DomainErrors.TrainRun.NotFound(query.TrainRunId);

        _trainCirculation = await trainCirculationRepository.GetAsync(_trainRun.CirculationId);
        if (_trainCirculation is null) throw new InvalidDataException("Train circulation not found for requested train run");

        if (_trainCirculation.ServiceInformation.ServiceNumber is null)
            return DomainErrors.TrainComposition.NotFindable;
        
        _route = await routeRepository.GetAsync(_trainRun.Id);
        if (_route is null) throw new InvalidDataException("Route not found for requested train run");
        
        _relevantStations = await stationRepository.GetManyAsync(_route.Stops.Select(s => s.StationId));
        
        var trainComposition = await trainCompositionRepository.GetAsync(_trainRun.Id);

        
        if (trainComposition is not null)
        {
            var updatedResult = await HandleExistingData(trainComposition);
            await unitOfWork.CommitAsync(cancellationToken);
            return updatedResult;
        }

        var result = await HandleUnknownData();
        await unitOfWork.CommitAsync(cancellationToken);
        return result;
    }

    private async Task<GetTrainCompositionResultDto> HandleExistingData(TrainComposition trainComposition)
    {
        if (_route is null) throw new InvalidOperationException("Cannot handle data without route being extracted");
        if(_relevantStations is null) throw new InvalidOperationException("Cannot handle data without relevant stations being extracted");
        if(_trainRun is null) throw new InvalidOperationException("Cannot handle data without train run being extracted");
        if(_trainCirculation?.ServiceInformation.ServiceNumber is null) throw new InvalidOperationException("Cannot handle data without train circulation being extracted with valid service number");
        
        var firstStop = _route.Stops.First();
        var firstStation = _relevantStations.First(s => s.Id == firstStop.StationId);
        var timeDifference = firstStop.DepartureTime!.Planned - DateTime.UtcNow;
        
        if (trainComposition.Source is TrainFormationSource.RealTime || timeDifference.TotalHours > 24)
        {
            var foundVehicles = await vehicleRepository.GetManyAsync(trainComposition.Vehicles.Select(v => v.VehicleId));
            return new TrainCompositionResultFactory(trainComposition, foundVehicles)
                .BuildResult();
        }
        
        var trainCompositionDto = await trainCompositionProvider.GetRealTimeDataAsync(
            _trainCirculation.ServiceInformation.ServiceNumber,
            _trainRun.OperatingDay.Date,
            firstStation.EvaNumber);

        if (trainCompositionDto is null)
        {
            var foundVehicles = await vehicleRepository.GetManyAsync(trainComposition.Vehicles.Select(v => v.VehicleId));
            return new TrainCompositionResultFactory(trainComposition, foundVehicles)
                .BuildResult();
        }
        
        var (relevantVehicles, formationSnapshots) = await ExtractVehicles(trainCompositionDto);
        trainComposition.Update(formationSnapshots);
        
        return new TrainCompositionResultFactory(trainComposition, relevantVehicles)
            .BuildResult();
    }

    private async Task<CanFail<GetTrainCompositionResultDto>> HandleUnknownData()
    {
        if (_route is null) throw new InvalidOperationException("Cannot handle data without route being extracted");
        if(_relevantStations is null) throw new InvalidOperationException("Cannot handle data without relevant stations being extracted");
        if(_trainRun is null) throw new InvalidOperationException("Cannot handle data without train run being extracted");
        if(_trainCirculation?.ServiceInformation.ServiceNumber is null) throw new InvalidOperationException("Cannot handle data without train circulation being extracted with valid service number");
        
        var firstStop = _route.Stops.First();
        var firstStation = _relevantStations.First(s => s.Id == firstStop.StationId);
        var timeDifference = firstStop.DepartureTime!.Planned - DateTime.UtcNow;
        
        //Past - reject
        if (timeDifference < -TimeSpan.FromHours(24))
        {
            return DomainErrors.TrainComposition.InPast;
        } 
        
        //Not in Future - try get realtime data
        if (timeDifference <= TimeSpan.FromHours(24))
        {
            var trainCompositionDto = await trainCompositionProvider.GetRealTimeDataAsync(
                _trainCirculation.ServiceInformation.ServiceNumber,
                _trainRun.OperatingDay.Date,
                firstStation.EvaNumber);

            if (trainCompositionDto is not null)
            {
                var (relevantVehicles, formationSnapshots) = await ExtractVehicles(trainCompositionDto);
                var trainComposition = TrainComposition.CreateFromRealtime(_trainRun.Id, formationSnapshots);
                trainCompositionRepository.Add(trainComposition);
                return new TrainCompositionResultFactory(trainComposition, relevantVehicles)
                    .BuildResult();
            }
        }
        //In Future or no realtime data available
        var lastStop = _route.Stops.Last();
        var lastStation = _relevantStations.First(s => s.Id == lastStop.StationId);
        
        //Try get planned data
        var plannedTrainCompositionDto = await trainCompositionProvider.GetPlannedDataAsync(
            _trainCirculation.ServiceInformation.ServiceNumber,
            firstStation.EvaNumber,
            firstStop.DepartureTime!.Planned,
            lastStation.EvaNumber,
            lastStop.ArrivalTime!.Planned);

        if (plannedTrainCompositionDto is not null)
        {
            var (relevantVehicles, formationSnapshots) = await ExtractVehicles(plannedTrainCompositionDto);
            var trainComposition = TrainComposition.CreateFromRealtime(_trainRun.Id, formationSnapshots);
            trainCompositionRepository.Add(trainComposition);
            return new TrainCompositionResultFactory(trainComposition, relevantVehicles)
                .BuildResult();
        }
        
        //Neither realtime nor planned data available - predict on history
        var predictedTrainCompositionDto = await predictor.PredictAsync(_trainCirculation.Id, _trainRun.OperatingDay);
        if (predictedTrainCompositionDto is not null)
        {
            var trainComposition = TrainComposition.CreateFromPrediction(_trainRun.Id, predictedTrainCompositionDto.PredictedComposition);
            trainCompositionRepository.Add(trainComposition);
            var relevantVehicles = await vehicleRepository.GetManyAsync(trainComposition.Vehicles.Select(v => v.VehicleId));
            return new TrainCompositionResultFactory(trainComposition, relevantVehicles)
                .BuildResult();
        }
        
        return DomainErrors.TrainComposition.NotFound;
    }

    private async Task<(List<Vehicle>, List<FormationVehicleSnapshot>)> ExtractVehicles(TrainCompositionDto trainCompositionDto)
    {
        if(_route is null) throw new InvalidOperationException("Cannot handle data without route being extracted");
        if (_relevantStations is null) throw new InvalidOperationException("Cannot handle data without relevant stations being extracted");
        
        var firstStationId = _route.Stops.First().StationId;
        var relevantVehicles = new List<Vehicle>();
        var formationVehicles = new List<FormationVehicleSnapshot>();
        
        foreach (var vehicle in trainCompositionDto.Vehicles)
        {
            var destinationStation = _relevantStations.First(station => station.Name == vehicle.DestinationStation);
            
            var coachSequence = vehicle.Coaches.Select(c => c.ConstructionType).ToList();
            
            var existingVehicle = relevantVehicles.FirstOrDefault(v => v.MatchesConstructionType(coachSequence));
            if (existingVehicle is null)
            {
                existingVehicle = await vehicleRepository.FindByConstructionTypeAsync(coachSequence);   
            }
            if (existingVehicle is null)
            {
                existingVehicle = Vehicle.CreateByConstructionType(vehicle.Name, coachSequence);
                vehicleRepository.Add(existingVehicle);
            }
            
            formationVehicles.Add(new FormationVehicleSnapshot(
                firstStationId,
                destinationStation.Id,
                existingVehicle.Id));
            relevantVehicles.Add(existingVehicle);
        }

        return (relevantVehicles, formationVehicles);
    }
    
    private async Task<(List<Vehicle>, List<FormationVehicleSnapshot>)> ExtractVehicles(PlannedTrainCompositionDto trainCompositionDto)
    {
        if(_route is null) throw new InvalidOperationException("Cannot handle data without route being extracted");
        if (_relevantStations is null) throw new InvalidOperationException("Cannot handle data without relevant stations being extracted");
        
        var firstStationId = _route.Stops.First().StationId;
        var lastStationId = _route.Stops.Last().StationId;
        var relevantVehicles = new List<Vehicle>();
        var formationVehicles = new List<FormationVehicleSnapshot>();
        
        foreach (var vehicle in trainCompositionDto.Vehicles)
        {
            var coachSequence = vehicle.Coaches.Select(c => c.ConstructionType).ToList();
            
            var existingVehicle = relevantVehicles.FirstOrDefault(v => v.MatchesCoachType(coachSequence));
            if (existingVehicle is null)
            {
                existingVehicle = await vehicleRepository.FindByCoachType(coachSequence);   
            }
            if (existingVehicle is null)
            {
                existingVehicle = Vehicle.CreateByCoachType(coachSequence);
                vehicleRepository.Add(existingVehicle);
            }
            
            formationVehicles.Add(new FormationVehicleSnapshot(
                firstStationId,
                lastStationId,
                existingVehicle.Id));
            relevantVehicles.Add(existingVehicle);
        }

        return (relevantVehicles, formationVehicles);
    }
}
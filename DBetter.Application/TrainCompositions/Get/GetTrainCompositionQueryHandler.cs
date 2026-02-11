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
    public override async Task<CanFail<GetTrainCompositionResultDto>> Handle(GetTrainCompositionQuery query, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);

        var trainRun = await trainRunRepository.GetAsync(query.TrainRunId);
        if (trainRun is null) return DomainErrors.TrainRun.NotFound(query.TrainRunId);

        var trainCirculation = await trainCirculationRepository.GetAsync(trainRun.CirculationId);
        if (trainCirculation is null) throw new InvalidDataException("Train circulation not found for requested train run");
        
        var route = await routeRepository.GetAsync(trainRun.Id);
        if (route is null) throw new InvalidDataException("Route not found for requested train run");
        
        var firstStation = await stationRepository.GetAsync(route.Stops.OrderBy(s => s.RouteIndex.Value).First().StationId);
        if(firstStation is null) throw new InvalidDataException("Station not found for requested route");

        if (trainCirculation.ServiceInformation.ServiceNumber is null)
            return Error.NotFound("TrainComposition.NotFindable",
                "The train composition cannot be found because no service number was found for the train circulation.");

        var trainComposition = await trainCompositionRepository.GetAsync(trainRun.Id);
        if (trainComposition is not null)
        {
            var foundVehicles = await vehicleRepository.GetManyAsync(trainComposition.Vehicles.Select(v => v.VehicleId));
            return new GetTrainCompositionResultDto
            {
                Vehicles = foundVehicles.Select(v => v.Identifier).ToList()
            };
        }
        
        var timeDifference = route.Stops.First().DepartureTime!.Planned - DateTime.UtcNow;
        
        //Past
        if (timeDifference < -TimeSpan.FromHours(24))
        {
            return Error.NotFound(
                "TrainComposition.InPast",
                "The requested train composition has not been found in the system and is older than 24h");
        } 
        //Future
        if (timeDifference > TimeSpan.FromHours(24))
        {
            return await GetPredictionAsync(trainCirculation.Id, trainRun.OperatingDay);
        }
        
        var trainCompositionDto = await trainCompositionProvider.GetRealTimeDataAsync(
            trainCirculation.ServiceInformation.ServiceNumber,
            trainRun.OperatingDay.Date,
            firstStation.EvaNumber);

        if (trainCompositionDto is null)
        {
            return await GetPredictionAsync(trainCirculation.Id, trainRun.OperatingDay);
        }

        if (trainCompositionDto.Vehicles.Any(tc => tc.Name.StartsWith("Zug")))
            return Error.Conflict(
                "TrainComposition.NotSupported",
                "Train compositions containing only wagons is currently not supported.");
        
        var destinationStations = await stationRepository.FindManyAsync(trainCompositionDto.Vehicles.Select(tc => tc.DestinationStation));
        
        var relevantVehicles = new List<Vehicle>();
        var formationVehicles = new List<FormationVehicleSnapshot>();
        
        foreach (var vehicle in trainCompositionDto.Vehicles)
        {
            var matchingStations = destinationStations.Where(s => s.Name == vehicle.DestinationStation).ToList();

            var destinationStation = matchingStations.First();
            
            //For stations with multiple ids like Berlin Hbf
            if (matchingStations.Count > 1)
            {
                destinationStation =
                    matchingStations.First(station => route.Stops.Select(s => s.StationId).Contains(station.Id));
            }
            
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
                firstStation.Id,
                destinationStation.Id,
                existingVehicle.Id));
            relevantVehicles.Add(existingVehicle);
        }

        trainComposition = TrainComposition.CreateFromRealtime(trainRun.Id, formationVehicles);
        trainCompositionRepository.Add(trainComposition);
        
        await unitOfWork.CommitAsync(cancellationToken);

        return new GetTrainCompositionResultDto
        {
            Vehicles = trainComposition.Vehicles
                .Select(fv => relevantVehicles.First(v => v.Id == fv.VehicleId).Identifier).ToList()
        };
    }

    private async Task<CanFail<GetTrainCompositionResultDto>> GetPredictionAsync(TrainCirculationId trainCirculationId, OperatingDay operatingDay)
    {
        var trainComposition = await predictor.PredictAsync(trainCirculationId, operatingDay);
        if (trainComposition is null)
            return Error.NotFound("TrainComposition.NotFound", "The requested train composition could not be found");
        
        var vehicles = await vehicleRepository.GetManyAsync(trainComposition.Vehicles.Select(v => v.VehicleId));
        
        return new GetTrainCompositionResultDto
        {
            Vehicles = trainComposition.Vehicles
                .Select(fv => vehicles.First(v => v.Id == fv.VehicleId).Identifier).ToList()
        };
    }
}
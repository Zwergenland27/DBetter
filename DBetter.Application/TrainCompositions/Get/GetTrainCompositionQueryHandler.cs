using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Contracts.TrainCompositions.Get;
using DBetter.Domain.Errors;
using DBetter.Domain.PlannedCoachLayouts.ValueObjects;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainCompositions.TrainParts;
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
        throw new NotImplementedException();
        // await unitOfWork.BeginTransaction(cancellationToken);
        //
        // _trainRun = await trainRunRepository.GetAsync(query.TrainRunId);
        // if (_trainRun is null) return DomainErrors.TrainRun.NotFound(query.TrainRunId);
        //
        // _trainCirculation = await trainCirculationRepository.GetAsync(_trainRun.CirculationId);
        // if (_trainCirculation is null) throw new InvalidDataException("Train circulation not found for requested train run");
        //
        // if (_trainCirculation.ServiceInformation.ServiceNumber is null)
        //     return DomainErrors.TrainComposition.NotFindable;
        //
        // _route = await routeRepository.GetAsync(_trainRun.Id);
        // if (_route is null) throw new InvalidDataException("Route not found for requested train run");
        //
        // _relevantStations = await stationRepository.GetManyAsync(_route.Stops.Select(s => s.StationId));
        //
        // var trainComposition = await trainCompositionRepository.GetAsync(_trainRun.Id);
        //
        // if (trainComposition is not null && trainComposition.Source is not TrainFormationSource.None)
        // {
        //     var updatedResult = await HandleExistingData(trainComposition);
        //     trainCompositionRepository.Save(trainComposition);
        //     await unitOfWork.CommitAsync(cancellationToken);
        //     return updatedResult;
        // }
        //
        // var result = await HandleUnknownData(trainComposition);
        // await unitOfWork.CommitAsync(cancellationToken);
        // return result;
    }

    private async Task GetPlannedDataAsync()
    {
        var resolver = new PlannedTrainPartIdentifier(_route.Stops.Select((s, i) => new RouteStopSnapshot(i, s.StationId)).ToList());
        var resolved = false;
        List<PlannedTrainPart>? plannedTrainParts;
        do
        {
            resolved = resolver.Resolve(out var departureStationToScrape, out var destinationStationToScrape, out plannedTrainParts);
            if (!resolved)
            {
                var departureStation = _relevantStations.First(s => s.Id == departureStationToScrape);
                var destinationStation = _relevantStations.First(s => s.Id == destinationStationToScrape);
                var departureCoachSequence = await trainCompositionProvider.GetPlannedDataAsync(
                   _trainCirculation.ServiceInformation.ServiceNumber,
                   departureStation.EvaNumber,
                   _route.Stops.First(s => s.StationId == departureStationToScrape).DepartureTime!.Planned,
                   destinationStation.EvaNumber,
                   _route.Stops.First(s => s.StationId == destinationStationToScrape).ArrivalTime!.Planned);
                
                var plannedCoachLayout = await GetPlannedCoachLayoutId(departureCoachSequence); 
                resolver.AddObservation(departureStation.Id, plannedCoachLayout);
            }
        } while (!resolved);
        
        //TODO: TrainComposition Objekt aus DB holen
        TrainComposition composition = null;
        composition.UpdatePlannedParts(plannedTrainParts);
    }

    private async Task<List<PlannedCoachLayoutId>> GetPlannedCoachLayoutId(PlannedTrainCompositionDto trainCompositionDto)
    {
        return [];
    }

    // private async Task<GetTrainCompositionResultDto> HandleExistingData(TrainComposition trainComposition)
    // {
    //     if (_route is null) throw new InvalidOperationException("Cannot handle data without route being extracted");
    //     if(_relevantStations is null) throw new InvalidOperationException("Cannot handle data without relevant stations being extracted");
    //     if(_trainRun is null) throw new InvalidOperationException("Cannot handle data without train run being extracted");
    //     if(_trainCirculation?.ServiceInformation.ServiceNumber is null) throw new InvalidOperationException("Cannot handle data without train circulation being extracted with valid service number");
    //     
    //     var firstStop = _route.Stops.First();
    //     var firstStation = _relevantStations.First(s => s.Id == firstStop.StationId);
    //     var timeDifference = firstStop.DepartureTime!.Planned - DateTime.UtcNow;
    //     
    //     if (trainComposition.Source is TrainFormationSource.RealTime || timeDifference.TotalHours > 8 || !trainComposition.IsNextCheckAllowed)
    //     {
    //         if (trainComposition.IsNextCheckAllowed)
    //         {
    //             trainComposition.ScheduleUpdate();
    //         }
    //         var foundVehicles = await vehicleRepository.GetManyAsync(trainComposition.ResolvedParts.Select(v => v.VehicleId));
    //         return new TrainCompositionResultFactory(trainComposition, foundVehicles)
    //             .BuildResult();
    //     }
    //     
    //     var trainCompositionDto = await trainCompositionProvider.GetRealTimeDataAsync(
    //         _trainCirculation.ServiceInformation.ServiceNumber,
    //         _trainRun.OperatingDay.Date,
    //         firstStation.EvaNumber);
    //
    //     if (trainCompositionDto is null)
    //     {
    //         trainComposition.ScheduleUpdate();
    //         var foundVehicles = await vehicleRepository.GetManyAsync(trainComposition.ResolvedParts.Select(v => v.VehicleId));
    //         return new TrainCompositionResultFactory(trainComposition, foundVehicles)
    //             .BuildResult();
    //     }
    //     
    //     var (relevantVehicles, formationSnapshots) = await ExtractVehicles(trainCompositionDto);
    //     trainComposition.UpdateFromRealTime(formationSnapshots);
    //     
    //     return new TrainCompositionResultFactory(trainComposition, relevantVehicles)
    //         .BuildResult();
    // }
    //
    // private async Task<CanFail<GetTrainCompositionResultDto>> HandleUnknownData(TrainComposition? baseObject)
    // {
    //     if (baseObject is not null && !baseObject.IsNextCheckAllowed)
    //     {
    //         baseObject.ScheduleUpdate();
    //         return DomainErrors.TrainComposition.NotFound;
    //     }
    //     if (_route is null) throw new InvalidOperationException("Cannot handle data without route being extracted");
    //     if(_relevantStations is null) throw new InvalidOperationException("Cannot handle data without relevant stations being extracted");
    //     if(_trainRun is null) throw new InvalidOperationException("Cannot handle data without train run being extracted");
    //     if(_trainCirculation?.ServiceInformation.ServiceNumber is null) throw new InvalidOperationException("Cannot handle data without train circulation being extracted with valid service number");
    //     
    //     var firstStop = _route.Stops.First();
    //     var firstStation = _relevantStations.First(s => s.Id == firstStop.StationId);
    //     var timeSinceDeparture = firstStop.DepartureTime!.Planned - DateTime.UtcNow;
    //     var timeSinceArrival = _route.Stops.Last().ArrivalTime!.Planned - DateTime.UtcNow;
    //     
    //     //Past - reject
    //     if (timeSinceArrival < -TimeSpan.FromHours(8))
    //     {
    //         return DomainErrors.TrainComposition.InPast;
    //     } 
    //     
    //     //Not in Future - try get realtime data
    //     if (timeSinceDeparture <= TimeSpan.FromHours(8))
    //     {
    //         var trainCompositionDto = await trainCompositionProvider.GetRealTimeDataAsync(
    //             _trainCirculation.ServiceInformation.ServiceNumber,
    //             _trainRun.OperatingDay.Date,
    //             firstStation.EvaNumber);
    //
    //         if (trainCompositionDto is not null)
    //         {
    //             var (relevantVehicles, formationSnapshots) = await ExtractVehicles(trainCompositionDto);
    //
    //             TrainComposition trainComposition;
    //             if (baseObject is not null)
    //             {
    //                 baseObject.UpdateFromRealTime(formationSnapshots);
    //                 trainComposition = baseObject;
    //             }
    //             else
    //             {
    //                 trainComposition = TrainComposition.CreateFromRealtime(_trainRun.Id, firstStop.DepartureTime, formationSnapshots);
    //                 trainCompositionRepository.Save(trainComposition);
    //             }
    //             
    //             return new TrainCompositionResultFactory(trainComposition, relevantVehicles)
    //                 .BuildResult();
    //         }
    //     }
    //     //In Future or no realtime data available
    //     var lastStop = _route.Stops.Last();
    //     var lastStation = _relevantStations.First(s => s.Id == lastStop.StationId);
    //     
    //     //Try get planned data
    //     var plannedTrainCompositionDto = await trainCompositionProvider.GetPlannedDataAsync(
    //         _trainCirculation.ServiceInformation.ServiceNumber,
    //         firstStation.EvaNumber,
    //         firstStop.DepartureTime!.Planned,
    //         lastStation.EvaNumber,
    //         lastStop.ArrivalTime!.Planned);
    //
    //     if (plannedTrainCompositionDto is not null)
    //     {
    //         var (relevantVehicles, formationSnapshots) = await ExtractVehicles(plannedTrainCompositionDto);
    //
    //         TrainComposition trainComposition;
    //         if (baseObject is not null)
    //         {
    //             baseObject.UpdateFromPlanned(formationSnapshots);
    //             trainComposition = baseObject;
    //         }
    //         else
    //         {
    //             trainComposition = TrainComposition.CreateFromPlanned(_trainRun.Id, firstStop.DepartureTime, formationSnapshots);
    //             trainCompositionRepository.Save(trainComposition);   
    //         }
    //         
    //         return new TrainCompositionResultFactory(trainComposition, relevantVehicles)
    //             .BuildResult();
    //     }
    //     
    //     //Neither realtime nor planned data available - predict on history
    //     var predictedTrainCompositionDto = await predictor.PredictAsync(_trainCirculation.Id, _trainRun.OperatingDay);
    //     if (predictedTrainCompositionDto is not null)
    //     {
    //         TrainComposition trainComposition;
    //         if (baseObject is not null)
    //         {
    //             baseObject.UpdateFromPrediction(predictedTrainCompositionDto.PredictedComposition);
    //             trainComposition = baseObject;
    //         }
    //         else
    //         {
    //             trainComposition = TrainComposition.CreateFromPrediction(_trainRun.Id, firstStop.DepartureTime, predictedTrainCompositionDto.PredictedComposition);
    //             trainCompositionRepository.Save(trainComposition);
    //         }
    //         var relevantVehicles = await vehicleRepository.GetManyAsync(trainComposition.ResolvedParts.Select(v => v.VehicleId));
    //         return new TrainCompositionResultFactory(trainComposition, relevantVehicles)
    //             .BuildResult();
    //     }
    //
    //     if (baseObject is null)
    //     {
    //         var trainComposition = TrainComposition.CreateNotFound(_trainRun.Id, firstStop.DepartureTime);
    //         trainCompositionRepository.Save(trainComposition);
    //     }
    //     
    //     return DomainErrors.TrainComposition.NotFound;
    // }
    //
    // private async Task<(List<Vehicle>, List<FormationVehicleSnapshot>)> ExtractVehicles(TrainCompositionDto trainCompositionDto)
    // {
    //     if(_route is null) throw new InvalidOperationException("Cannot handle data without route being extracted");
    //     if (_relevantStations is null) throw new InvalidOperationException("Cannot handle data without relevant stations being extracted");
    //     
    //     var firstStationId = _route.Stops.First().StationId;
    //     var relevantVehicles = new List<Vehicle>();
    //     var formationVehicles = new List<FormationVehicleSnapshot>();
    //     
    //     foreach (var vehicle in trainCompositionDto.Vehicles)
    //     {
    //         var destinationStation = _relevantStations.FirstOrDefault(station => station.Name == vehicle.DestinationStation) ??
    //                                  _relevantStations.First(station => station.Id == _route.Stops.Last().StationId); //Fallback for the case the destination station is the short station name
    //
    //         var coachSequence = vehicle.Coaches.Select(c => c.ConstructionType).ToList();
    //         
    //         var existingVehicle = relevantVehicles.FirstOrDefault(v => v.MatchesConstructionType(coachSequence));
    //         if (existingVehicle is null)
    //         {
    //             existingVehicle = await vehicleRepository.FindByConstructionTypeAsync(coachSequence);   
    //         }
    //         if (existingVehicle is null)
    //         {
    //             existingVehicle = Vehicle.CreateByConstructionType(vehicle.Name, coachSequence);
    //             vehicleRepository.Add(existingVehicle);
    //         }
    //         
    //         formationVehicles.Add(new FormationVehicleSnapshot(
    //             firstStationId,
    //             destinationStation.Id,
    //             existingVehicle.Id));
    //         relevantVehicles.Add(existingVehicle);
    //     }
    //
    //     return (relevantVehicles, formationVehicles);
    // }
    //
    // private async Task<(List<Vehicle>, List<FormationVehicleSnapshot>)> ExtractVehicles(PlannedTrainCompositionDto trainCompositionDto)
    // {
    //     if(_route is null) throw new InvalidOperationException("Cannot handle data without route being extracted");
    //     if (_relevantStations is null) throw new InvalidOperationException("Cannot handle data without relevant stations being extracted");
    //     
    //     var firstStationId = _route.Stops.First().StationId;
    //     var lastStationId = _route.Stops.Last().StationId;
    //     var relevantVehicles = new List<Vehicle>();
    //     var formationVehicles = new List<FormationVehicleSnapshot>();
    //     
    //     foreach (var vehicle in trainCompositionDto.Vehicles)
    //     {
    //         var coachSequence = vehicle.Coaches.Select(c => c.ConstructionType).ToList();
    //         
    //         var existingVehicle = relevantVehicles.FirstOrDefault(v => v.MatchesCoachType(coachSequence));
    //         if (existingVehicle is null)
    //         {
    //             existingVehicle = await vehicleRepository.FindByCoachType(coachSequence);   
    //         }
    //         if (existingVehicle is null)
    //         {
    //             existingVehicle = Vehicle.CreateByCoachType(coachSequence);
    //             vehicleRepository.Add(existingVehicle);
    //         }
    //         
    //         formationVehicles.Add(new FormationVehicleSnapshot(
    //             firstStationId,
    //             lastStationId,
    //             existingVehicle.Id));
    //         relevantVehicles.Add(existingVehicle);
    //     }
    //
    //     return (relevantVehicles, formationVehicles);
    // }
}
using CleanDomainValidation.Domain;
using CleanMediator.Commands;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.Connections.Dtos;
using DBetter.Application.TrainCompositions;
using DBetter.Application.TrainRuns.Dtos;
using DBetter.Contracts.TrainRuns.Queries.Get.Results;
using DBetter.Domain.Errors;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Routes;
using DBetter.Domain.Routes.Snapshots;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.Snapshots;

namespace DBetter.Application.TrainRuns.Queries.Get;

public class GetTrainRunQueryHandler(
    IUnitOfWork unitOfWork,
    IStationRepository stationRepository,
    ITrainRunRepository trainRunRepository,
    IRouteRepository routeRepository,
    ITrainCirculationRepository trainCirculationRepository,
    IExternalTrainRunProvider trainRunProvider,
    IPassengerInformationRepository passengerInformationRepository,
    ITrainCompositionQueryRepository trainCompositionRepository) : CommandHandlerBase<GetTrainRunQuery, TrainRunResponse>
{
    private List<Station> _existingStations = [];
    private List<Station> _stationsToCreate = [];
    private List<PassengerInformation> _existingPassengerInformation = [];
    private List<PassengerInformation> _passengerInformationToCreate = [];
    public override async Task<CanFail<TrainRunResponse>> Handle(GetTrainRunQuery request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var trainRun = await trainRunRepository.GetAsync(request.Id);
        if (trainRun is null) return DomainErrors.TrainRun.NotFound(request.Id);

        var route = await routeRepository.GetAsync(request.Id);
        if (route is null) throw new InvalidDataException("No route exists for the train run");
        
        var trainCirculation = await trainCirculationRepository.GetAsync(trainRun.CirculationId);
        if (trainCirculation is null) throw new InvalidDataException("No train circulation exists for the train run");
        
        var trainRunDto = await trainRunProvider.GetTrainRunAsync(trainRun.JourneyId);
        _existingPassengerInformation = await passengerInformationRepository.GetManyAsync(trainRun.PassengerInformation.Select(im => im.InformationId));
        _existingPassengerInformation.AddRange(await passengerInformationRepository.FindManyAsync(trainRunDto.PassengerInformation.Select(pim => pim.OriginalText)));
        _existingPassengerInformation = _existingPassengerInformation.Distinct().ToList();
        
        await ExtractStations(trainRunDto);
        ExtractMissingStations(trainRunDto);
        ExtractMissingPassengerInformation(trainRunDto);
        
        stationRepository.AddRange(_stationsToCreate);
        passengerInformationRepository.AddRange(_passengerInformationToCreate);
        
        trainRun.Update(trainRunDto.BikeCarriage);
        trainRun.Update(trainRunDto.Catering);
        
        route.UpdateFromTrainRun(ExtractStops(trainRunDto.Stops));
        
        var passengerInformationSnapshots = trainRunDto.PassengerInformation
            .Select(pimDto =>
            {
                var pim = _existingPassengerInformation.First(im => im.Text == pimDto.OriginalText);
                return new TrainRunPassengerInformationSnapshot(pim.Id, pimDto.FromStopIndex, pimDto.ToStopIndex);
            }).ToList();
        trainRun.ReconcilePassengerInformation(passengerInformationSnapshots);
        
        if (trainRunDto.ServiceNumbers.Any())
        {
            trainCirculation.Update(trainRunDto.ServiceNumbers.First());   
        }
        
        await unitOfWork.CommitAsync(cancellationToken);

        var trainComposition = await trainCompositionRepository.GetAsync(trainRun.Id);
        
        var responseFactory = new TrainRunResponseFactory(trainCirculation, trainRun, _existingPassengerInformation, _existingStations);
        return responseFactory.MapToResponse(trainRunDto, trainComposition);
    }

    private async Task ExtractStations(TrainRunDto trainRunDto)
    {
        var evaNumbers = trainRunDto.Stops.Select(stop => stop.EvaNumber);
        _existingStations = await stationRepository.GetManyAsync(evaNumbers);
    }
    
    private void ExtractMissingStations(TrainRunDto trainRunDto)
    {
        var unknownStations = trainRunDto.GetUnknownStations(_existingStations);
        foreach (var station in unknownStations)
        {
            var newStation = Station.Create(station.EvaNumber, station.Name, station.InfoId);
            _stationsToCreate.Add(newStation);
            _existingStations.Add(newStation);
        }
    }
    
    private void ExtractMissingPassengerInformation(TrainRunDto trainRunDto)
    {
        var unknownPassengerInformation = trainRunDto.GetUnknownPassengerInformation(_existingPassengerInformation);
        
        foreach (var passengerInformation in unknownPassengerInformation)
        {
            var newInfoMessage = PassengerInformation.FoundNew(passengerInformation.OriginalText, passengerInformation.Priority);
            _passengerInformationToCreate.Add(newInfoMessage);
            _existingPassengerInformation.Add(newInfoMessage);
        }
    }
    
    private List<StopSnapshot> ExtractStops(List<StopDto> stopDtos)
    {
        var stops =  new List<StopSnapshot>();

        foreach (var stop in stopDtos)
        {
            var station = _existingStations.First(s => s.EvaNumber == stop.EvaNumber);
            stops.Add(new StopSnapshot(
                stop.TrainRunIndex,
                station.Id,
                stop.ArrivalTime,
                stop.DepartureTime,
                stop.Attributes));
        }
        
        return stops;
    }
}
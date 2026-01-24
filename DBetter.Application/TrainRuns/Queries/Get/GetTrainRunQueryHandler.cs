using CleanDomainValidation.Domain;
using CleanMediator.Commands;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.TrainRuns.Dtos;
using DBetter.Contracts.TrainRuns.Queries.Get.Results;
using DBetter.Domain.Errors;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.Snapshots;

namespace DBetter.Application.TrainRuns.Queries.Get;

public class GetTrainRunQueryHandler(
    IUnitOfWork unitOfWork,
    IStationRepository stationRepository,
    ITrainRunRepository trainRunRepository,
    IExternalTrainRunProvider trainRunProvider,
    IPassengerInformationRepository passengerInformationRepository) : CommandHandlerBase<GetTrainRunQuery, TrainRunResponse>
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
        
        var passengerInformationSnapshots = trainRunDto.PassengerInformation
            .Select(pimDto =>
            {
                var pim = _existingPassengerInformation.First(im => im.Text == pimDto.OriginalText);
                return new TrainRunPassengerInformationSnapshot(pim.Id, pimDto.FromStopIndex, pimDto.ToStopIndex);
            }).ToList();
        trainRun.ReconcilePassengerInformation(passengerInformationSnapshots);
        
        if (trainRunDto.ServiceNumbers.Any())
        {
            trainRun.Update(trainRunDto.ServiceNumbers.First());   
        }
        
        await unitOfWork.CommitAsync(cancellationToken);

        var responseFactory = new TrainRunResponseFactory(trainRun, _existingStations);
        return responseFactory.MapToResponse(trainRunDto);
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
}
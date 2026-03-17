using CleanDomainValidation.Domain;
using CleanMediator.Commands;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Domain.Errors;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.Stations.ScrapeDepartures;

public class ScrapeStationDeparturesCommandHandler(
    IUnitOfWork unitOfWork,
    IStationRepository stationRepository,
    IExternalDepartureProvider departureProvider,
    ITrainCirculationRepository trainCirculationRepository,
    ITrainRunRepository trainRunRepository,
    IRouteRepository routeRepository): CommandHandlerBase<ScrapeStationDeparturesCommand>
{
    public override async Task<CanFail> Handle(ScrapeStationDeparturesCommand command, CancellationToken cancellationToken)
    {
        var station = await stationRepository.GetAsync(command.StationId);
        if (station is null) return DomainErrors.Station.NotFound;

        var departures = await departureProvider.GetDepartures(station.EvaNumber, command.ForDay);

        foreach (var departure in departures)
        {
            if (departure.ServiceInformation.ServiceNumber is not null && departure.ServiceInformation.ServiceNumber.IsReplacement)
            {
                continue;
            }
            await HandleDeparture(departure);
        }
        
        return CanFail.Success;
    }

    private async Task HandleDeparture(DepartureDto departureDto)
    {
        await unitOfWork.BeginTransaction();
        
        var trainCirculation = await trainCirculationRepository.GetAsync(departureDto.JourneyId.TimeTableCompositeIdentifier);
        if (trainCirculation is null)
        {
            trainCirculation = TrainCirculation.Create(
                departureDto.JourneyId,
                departureDto.ServiceInformation);
        }
        else
        {
            trainCirculation.Update(departureDto.ServiceInformation);
        }
        
        trainCirculationRepository.Save(trainCirculation);

        var trainRun = await trainRunRepository.GetAsync(departureDto.JourneyId.TrainRunCompositeIdentifier);
        if (trainRun is not null)
        {
            await unitOfWork.CommitAsync();
            return;
        }
        
        trainRun = TrainRunFactory.Create(trainCirculation, departureDto.JourneyId, [], BikeCarriageInformation.Unknown, CateringInformation.Unknown);
        trainRunRepository.Add(trainRun);

        var route = Route.CreateEmpty(trainRun.Id);
        routeRepository.Add(route);
        
        await unitOfWork.CommitAsync();
    }
}
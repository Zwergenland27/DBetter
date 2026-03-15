using DBetter.Application.Abstractions.Persistence;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Departures;
using DBetter.Infrastructure.BahnDe.Shared;
using DBetter.Infrastructure.Postgres;
using DBetter.Infrastructure.TrainCirculations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DBetter.Infrastructure.Jobs;

public class ScrapeLongDistanceTrainsJob(
    ILogger<ScrapeLongDistanceTrainsJob> logger,
    IUnitOfWork unitOfWork,
    DBetterContext db,
    DepartureProvider departureProvider,
    ITrainCirculationRepository trainCirculationRepository) : IJob
{
    public static JobKey JobKey = new JobKey("ScrapeLongDistanceTrainsJob");
    private readonly List<StationId> MostImportantStations =
    [
        StationId.Create("1da5ecec-ee8c-43b4-8d12-e7d7a7603af4").Value, //Frankfurt Main
        StationId.Create("89259886-ced4-47d0-beb6-f27f50afd906").Value,  //Hamburg
        StationId.Create("49996716-8eb0-4545-83b5-319c3d83b65a").Value, //München
        StationId.Create("7afe3925-600d-49a4-b5b8-ea2ad3a9f948").Value, //Berlin
        StationId.Create("21421ce1-6e49-4490-b8ec-5c22a906b75a").Value, //Berlin
        StationId.Create("0d34d32d-7f8e-4b91-b2b1-f6fc4dd84c40").Value, //Köln
        StationId.Create("689dde58-08ff-4481-8663-65e185a1e5d3").Value, //Hannover
        StationId.Create("20da42fe-45c1-40ce-be00-9562a62f0f48").Value, //Mannheim
        StationId.Create("635e5ae3-34fc-4719-9203-ac01e04c7d3b").Value, //Stuttgart
        StationId.Create("4401f7e5-94a9-4fa5-bf89-73d22ab0710f").Value, //Düsseldorf
        StationId.Create("087a866f-201e-4bce-ab1f-99407c86a469").Value, //Nürnberg
        StationId.Create("684f4c68-e14f-4065-8309-1ebedb172ce9").Value, //Leipzig
        StationId.Create("ef2ef3b2-1513-4ab4-82c5-4cfe663418f8").Value //Dortmund
    ];
    private readonly List<BahnJourneyId> _checkedJourneyIds = [];
    public async Task Execute(IJobExecutionContext context)
    {
        var longDistanceAvailableStationEvas = await db.Stations
            .AsNoTracking()
            .Where(station => MostImportantStations.Contains(station.Id))
            .Select(station => station.EvaNumber)
            .ToListAsync();
        
        logger.LogInformation("Start scraping {count} stations", longDistanceAvailableStationEvas.Count);
        foreach (var stationEva in longDistanceAvailableStationEvas)
        {
            await ScrapeStation(stationEva);
            break;
        }
        logger.LogInformation("Station scraping complete, continue processing train runs in background.");
    }

    private async Task ScrapeStation(EvaNumber stationEva)
    {
        try
        {
            var today = DateTime.Today;
            var departures = new List<Abfahrt>();
            for (var i = 0; i < 23; i++)
            {
                var requestTime = today.AddHours(i);
                var abfahrten = await departureProvider.GetJourneyIds(stationEva, requestTime);
                departures.AddRange(abfahrten);
            }

            departures = departures.DistinctBy(d => d.JourneyId).ToList();

            var filteredDepartures = departures
                .Where(departure => !_checkedJourneyIds.Contains(BahnJourneyId.Create(departure.JourneyId)))
                .ToList();

            var existingJourneyIds = await db.TrainRuns
                .AsNoTracking()
                .Where(trainRun => filteredDepartures.Select(departure => BahnJourneyId.Create(departure.JourneyId))
                    .Contains(trainRun.JourneyId))
                .Select(trainRun => trainRun.JourneyId)
                .ToListAsync();

            _checkedJourneyIds.AddRange(existingJourneyIds);
            filteredDepartures.RemoveAll(departure =>
                existingJourneyIds.Contains(BahnJourneyId.Create(departure.JourneyId)));

            foreach (var departure in filteredDepartures)
            {
                await ScrapeDeparture(departure);
            }

            _checkedJourneyIds.AddRange(filteredDepartures.Select(departure => BahnJourneyId.Create(departure.JourneyId)));
        }
        catch (Exception e)
        {
            logger.LogError(0, e, "Error while scraping station {evaNumber}", stationEva.Value);
        }

    }

    private async Task ScrapeDeparture(Abfahrt departure)
    {
        await unitOfWork.BeginTransaction();
        var journeyId = BahnJourneyId.Create(departure.JourneyId);
        var operatingDay = journeyId.OperatingDay;
        var trainId = journeyId.TrainId;
        var timeTablePeriod = TimeTablePeriod.FromOperatingDay(operatingDay);

        //TODO: This will not work for local trains since the "LangText" contains only the service number
        var serviceInformation =
            new LineInformationFactory(departure.Verkehrmittel.ProduktGattung, departure.Verkehrmittel.LangText);
        
        var trainCirculationPersistence = await db.TrainCirculations
            .FirstOrDefaultAsync(trainCirculation => trainCirculation.TimeTablePeriod == timeTablePeriod.Year && trainCirculation.TrainId == trainId.Value);

        TrainCirculation? trainCirculation;
        if (trainCirculationPersistence is null)
        {
            trainCirculation = TrainCirculation.Create(
                journeyId,
                serviceInformation.ExtractData());
        }
        else
        {
            trainCirculation = trainCirculationPersistence.ToDomain();
        }

        if (departure.Verkehrmittel.LinienNummer is not null)
        {
            trainCirculation.Update(LineNumber.Create(departure.Verkehrmittel.LinienNummer));
        }

        var trainRun = TrainRunFactory.Create(
            trainCirculation,
            journeyId,
            [],
            BikeCarriageInformation.Unknown,
            CateringInformation.Unknown);
        db.TrainRuns.Add(trainRun);

        var route = Route.CreateEmpty(trainRun.Id);
        db.Routes.Add(route);
        
        trainCirculationRepository.Save(trainCirculation);

        await unitOfWork.CommitAsync();
    }
}
using DBetter.Application.Abstractions.Persistence;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Infrastructure.BahnDe.Departures;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DBetter.Infrastructure.Jobs;

public class ScrapeLongDistanceTrainsJob(
    ILogger<ScrapeLongDistanceTrainsJob> logger,
    IUnitOfWork unitOfWork,
    DBetterContext db) : IJob
{
    public static JobKey JobKey = new JobKey("ScrapeLongDistanceTrainsJob");
    private readonly List<StationId> _mostImportantStationIds =
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
    public async Task Execute(IJobExecutionContext context)
    {
        await unitOfWork.BeginTransaction();
        var mostImportantStations = await db.Stations
            .Where(station => _mostImportantStationIds.Contains(station.Id))
            .ToListAsync();
        
        var germanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
        var nowInGermany = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, germanTimeZone);
        var today = DateOnly.FromDateTime(nowInGermany);
        var inTwoWeeks = today.AddDays(14);
        
        logger.LogInformation("Scheduled scraping for {Count} stations for the {Date}",  mostImportantStations.Count, inTwoWeeks);

        foreach (var station in mostImportantStations)
        {
            station.ScrapeDepartures(inTwoWeeks);
        }

        await unitOfWork.CommitAsync();
    }
}
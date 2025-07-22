using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.ApiMarketplace.StaDa;
using DBetter.Infrastructure.ApiMarketplace.Timetables;
using DBetter.Infrastructure.BahnDe.Stations;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace DBetter.Infrastructure.BackgroundJobs;

public class StationScraperJob(
    DBetterContext database,
    StaDaService stada,
    TimetablesService timetables,
    StationService stationService) : IJob
{
    public static JobKey JobKey => JobKey.Create(nameof(StationScraperJob));
    
    public async Task Execute(IJobExecutionContext context)
    {
        var station = await database.Stations
            .FirstOrDefaultAsync(station =>
                station.LastScrapedAt == null && (station.Position == null || station.InfoId == null || station.Ril100 == null));

        if (station is null) return;

        var success = await GetStadaInformation(station);
        if (!success)
        {
            await GetOtherSourceInformation(station);
        }
        
        await database.SaveChangesAsync();
    }

    private async Task<bool> GetStadaInformation(Station station)
    {
        var results = await stada.GetStationData(station.EvaNumber.Value);
        if (results.Count < 1) return false;
        var stadaInformation = results[0];
        
        Coordinates? position = null;
        StationInfoId? stationInfoId = null;
        Ril100? ril100 = null;
        
        var coordinates = stadaInformation.EvaNumbers
            .FirstOrDefault(evaNumber => evaNumber.IsMain)
            ?.GeographicCoordinates.Coordinates;
        
        if (coordinates is not null)
        {
            position = new Coordinates(coordinates[0], coordinates[1]);
        }
        
        var stationInfoResult = StationInfoId.Create(stadaInformation.Number.ToString());
        if (!stationInfoResult.HasFailed)
        {
            stationInfoId = stationInfoResult.Value;
        }

        var ril100Identifier = stadaInformation.Ril100Identifiers
            .FirstOrDefault(ril100 => ril100.IsMain)
            ?.RilIdentifier;
        if (ril100Identifier is not null)
        {
            ril100 = Ril100.Create(ril100Identifier);
        }
        
        station.UpdateScrapedInformation(position, stationInfoId, ril100);
        return true;
    }

    private async Task GetOtherSourceInformation(Station station)
    {
        Coordinates? position = null;
        Ril100? ril100 = null;
        
        var bahnResults = await stationService.FindAsync(station.EvaNumber.Value);
        var bahnDeInformation = bahnResults
            .FirstOrDefault(s => s.ExtId == station.EvaNumber.Value)
            ?.ToDomain();
        if (bahnDeInformation is not null)
        {
            position = bahnDeInformation.Position;
        }

        var timetablesResults = await timetables.GetStationData(station.EvaNumber.Value);
        var timetableInformation = timetablesResults
            .FirstOrDefault();
        
        if (timetableInformation is not null)
        {
            ril100 = Ril100.Create(timetableInformation.Ds100);
        }
        
        station.UpdateScrapedInformation(position, null, ril100);
    }
}
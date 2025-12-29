using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.ApiMarketplace.StaDa;
using DBetter.Infrastructure.ApiMarketplace.Timetables;
using DBetter.Infrastructure.BahnDe.Stations;

namespace DBetter.Infrastructure.Repositories;

public class StationInfoProvider(
    StaDaService stada,
    TimetablesService timetables,
    StationService stationService) : IStationInfoProvider
{
    public async Task<StationInformation> GetStationInfoAsync(EvaNumber evaNumber)
    {
        Coordinates? location = null;
        StationInfoId? stationInfoId = null;
        Ril100? ril100 = null;

        var stadaResult = await TryGetStadaInformation(evaNumber);

        if (stadaResult is not null)
        {
            location = stadaResult.Value.Position;
            stationInfoId = stadaResult.Value.StationInfoId;
            ril100 = stadaResult.Value.Ril100;
        }

        location ??= await TryGetLocation(evaNumber);
        ril100 ??= await TryGetRil100(evaNumber);

        return new StationInformation(
            location,
            stationInfoId,
            ril100);
    }

    private async Task<(Coordinates? Position, StationInfoId? StationInfoId, Ril100? Ril100)?> TryGetStadaInformation(EvaNumber evaNumber)
    {
        var results = await stada.GetStationData(evaNumber.Value);
        if (results.Count < 1) return null;
        var stadaInformation = results[0];
        
        Coordinates? position = null;
        StationInfoId? stationInfoId = null;
        Ril100? ril100 = null;
        
        var coordinates = stadaInformation.EvaNumbers
            .FirstOrDefault(eva => eva.IsMain)
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
            .FirstOrDefault(identifier => identifier.IsMain)
            ?.RilIdentifier;
        if (ril100Identifier is not null)
        {
            ril100 = Ril100.Create(ril100Identifier);
        }
        
        return (position, stationInfoId, ril100);
    }

    private async Task<Coordinates?> TryGetLocation(EvaNumber evaNumber)
    {
        var stationResults = await stationService.FindAsync(evaNumber.Value);
        var stationData = stationResults.FirstOrDefault(station => station.ExtId == evaNumber.Value);

        if (stationData is null)
        {
            return null;
        }

        return new Coordinates(
            stationData.Lat,
            stationData.Lon);
    }

    private async Task<Ril100?> TryGetRil100(EvaNumber evaNumber)
    {
        var timetablesResults = await timetables.GetStationData(evaNumber.Value);
        var timetableInformation = timetablesResults
            .FirstOrDefault();
        
        if (timetableInformation is not null)
        {
            return Ril100.Create(timetableInformation.Ds100);
        }

        return null;
    }
}
using DBetter.Application.Abstractions.Caching;
using DBetter.Application.Stations;
using DBetter.Application.Stations.Dtos;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.ApiMarketplace.StaDa;
using DBetter.Infrastructure.ApiMarketplace.Timetables;
using DBetter.Infrastructure.BahnDe.Stations;

namespace DBetter.Infrastructure.Repositories;

public class ExternalStationProvider(
    ICache cache,
    StaDaService stada,
    TimetablesService timetables,
    StationService stationService) : IExternalStationProvider
{
    public async Task<StationProviderDto> GetStationInfoAsync(EvaNumber evaNumber)
    {
        Coordinates? location = null;
        StationInfoId? stationInfoId = null;
        Ril100Identifier? ril100 = null;

        var stadaResult = await TryGetStadaInformation(evaNumber);

        if (stadaResult is not null)
        {
            location = stadaResult.Value.Position;
            stationInfoId = stadaResult.Value.StationInfoId;
            ril100 = stadaResult.Value.Ril100;
        }

        location ??= await TryGetLocation(evaNumber);
        ril100 ??= await TryGetRil100(evaNumber);

        return new StationProviderDto(
            location,
            stationInfoId,
            ril100);
    }

    public async Task<List<StationQueryDto>> FindAsync(string query)
    {
        if (cache.TryGetValue<List<StationQueryDto>>($"stationProvider:find:{query}", out var stations))
        {
            return stations;
        }
        
        var haltestellen = await stationService.FindAsync(query, 5);
        
        var dtos = haltestellen
            .Select(halt => halt.ToSnapshot())
            .OfType<StationQueryDto>()
            .ToList();
        
        cache.Set($"stationProvider:find:{query}", dtos, new CachingOptions
        {
            Duration = TimeSpan.FromHours(8)
        });

        return dtos;
    }

    private async Task<(Coordinates? Position, StationInfoId? StationInfoId, Ril100Identifier? Ril100)?> TryGetStadaInformation(EvaNumber evaNumber)
    {
        var results = await stada.GetStationData(evaNumber.Value);
        if (results.Count < 1) return null;
        var stadaInformation = results[0];
        
        Coordinates? position = null;
        StationInfoId? stationInfoId = null;
        Ril100Identifier? ril100 = null;
        
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
            var ril100Result =  Ril100Identifier.Create(ril100Identifier);
            if (ril100Result.HasFailed) return null;
            ril100 = ril100Result.Value;
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

    private async Task<Ril100Identifier?> TryGetRil100(EvaNumber evaNumber)
    {
        var timetablesResults = await timetables.GetStationData(evaNumber.Value);
        var timetableInformation = timetablesResults
            .FirstOrDefault();
        
        if (timetableInformation is not null)
        {
            var ril100Result =  Ril100Identifier.Create(timetableInformation.Ds100);
            if (ril100Result.HasFailed) return null;
            return ril100Result.Value;
        }

        return null;
    }
}
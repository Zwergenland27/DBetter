using DBetter.Application.Stations;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Stations;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using StationDto = DBetter.Contracts.Stations.Queries.Find.StationDto;

namespace DBetter.Infrastructure.Repositories;

public class StationQueryRepository(
    StationService stationService,
    DBetterContext context) : IStationQueryRepository
{
    public async Task<List<StationDto>> FindAsync(string query)
    {
        var haltestellen = await stationService.FindAsync(query, 5);

        var recievedStations = haltestellen
            .Select(halt => halt.ToDomain())
            .OfType<Station>()
            .ToList();

        var stations = await InsertStations(recievedStations);
        
        await context.SaveChangesAsync();

        var ril100MatchingStation = await context.Stations
            .FirstOrDefaultAsync(station => station.Ril100 == Ril100.Create(query.ToUpper()));
        if (ril100MatchingStation is not null)
        {
            stations.Insert(0, ril100MatchingStation);
        }
        
        return stations.Select(station => new StationDto
        {
            Id = station.Id.Value.ToString(),
            Name = station.Name.NormalizedValue,
            Ril100 = station.Ril100?.Value,
        }).ToList();
    }

    private async Task<List<Station>> InsertStations(List<Station> fetchedStations)
    {
        var stationEvaNumbers = fetchedStations.Select(s => s.EvaNumber).ToList();
        var existingStations = await context.Stations
            .AsNoTracking()
            .Where(station => stationEvaNumbers.Contains(station.EvaNumber))
            .ToListAsync();

        List<Station> stations = existingStations;

        foreach (var station in fetchedStations)
        {
            var existingStation = existingStations.SingleOrDefault(s => s.EvaNumber == station.EvaNumber);
            if (existingStation is null)
            {
                await context.Stations.AddAsync(station);
                stations.Add(station);
            }
        }

        return stations;
    }
}
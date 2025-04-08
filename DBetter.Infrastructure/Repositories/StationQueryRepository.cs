using DBetter.Application.Stations;
using DBetter.Domain.Stations;
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

        var stations = await InsertOrUpdateStations(recievedStations);
        
        return stations.Select(station => new StationDto
        {
            Id = station.Id.Value.ToString(),
            Name = station.Name.Value,
        }).ToList();
    }

    private async Task<List<Station>> InsertOrUpdateStations(List<Station> fetchedStations)
    {
        var stationEvaNumbers = fetchedStations.Select(s => s.EvaNumber).ToList();
        var existingStations = await context.Stations
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
            else if(station.Position is not null)
            {
                existingStation.UpdatePosition(station.Position);
            }
        }

        return stations;
    }
}
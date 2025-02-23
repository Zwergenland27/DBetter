using DBetter.Application.Stations;
using DBetter.Domain.Stations;
using DBetter.Infrastructure.BahnDe.Stations;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using StationDto = DBetter.Contracts.Stations.Queries.Find.StationDto;

namespace DBetter.Infrastructure.Repositories;

public class StationQueryRepository(
    StationService service,
    DBetterContext context) : IStationQueryRepository
{
    public async Task<List<StationDto>> FindAsync(string query)
    {
        var stations = await service.FindAsync(query, 5);
        
        await InsertOrUpdateStations(stations);
        
        return stations.Select(station => new StationDto
        {
            EvaNumber = station.EvaNumber.Value,
            Name = station.Name.Value,
        }).ToList();
    }

    private async Task InsertOrUpdateStations(List<Station> fetchedStations)
    {
        var stationEvaNumbers = fetchedStations.Select(s => s.EvaNumber).ToList();
        var existingStations = await context.Stations
            .Where(station => stationEvaNumbers.Contains(station.EvaNumber))
            .ToListAsync();

        foreach (var station in fetchedStations)
        {
            var existingStation = existingStations.SingleOrDefault(s => s.EvaNumber == station.EvaNumber);
            if (existingStation is null)
            {
                await context.Stations.AddAsync(station);
            }
            else if(station.Position is not null)
            {
                existingStation.UpdatePosition(station.Position);
            }
        }
        
        await context.SaveChangesAsync();
    }
}
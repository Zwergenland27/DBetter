using DBetter.Application.Stations;
using DBetter.Infrastructure.BahnDe.Stations;
using StationDto = DBetter.Contracts.Stations.Queries.Find.StationDto;

namespace DBetter.Infrastructure.Repositories;

public class StationQueryRepository(StationService service) : IStationQueryRepository
{
    public async Task<List<StationDto>> FindAsync(string query)
    {
        var stations = await service.FindAsync(query, 5);
        return stations.Select(station => new StationDto
        {
            EvaNumber = station.EvaNumber.Value,
            Name = station.Name.Value,
        }).ToList();
    }
}
using DBetter.Contracts.Stations.Queries.Find;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Stations;

public interface IStationQueryRepository
{
    Task<List<StationDto>> FindAsync(string query);
}
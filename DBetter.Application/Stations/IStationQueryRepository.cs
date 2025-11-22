using DBetter.Contracts.Stations.Queries.Find;
namespace DBetter.Application.Stations;

public interface IStationQueryRepository
{
    Task<List<StationDto>> FindAsync(string query);
}
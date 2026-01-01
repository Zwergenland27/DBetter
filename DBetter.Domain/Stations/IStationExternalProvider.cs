using DBetter.Domain.Stations.Snapshots;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public interface IStationExternalProvider
{
    Task<StationInformation> GetStationInfoAsync(EvaNumber evaNumber);
    
    Task<List<StationQuerySnapshot>> FindAsync(string query);
}
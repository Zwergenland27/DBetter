using DBetter.Domain.Stations.Snapshots;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public interface IStationRepository
{
    Task<Station?> GetAsync(StationId id);

    Task<Station?> FindByRil100Async(Ril100Identifier ril100Identifier);
    
    Task<List<Station>> GetManyAsync(IEnumerable<EvaNumber> evaNumbers);
    
    void AddRange(IEnumerable<Station> stations);
}
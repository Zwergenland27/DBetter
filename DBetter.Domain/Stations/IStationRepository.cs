using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public interface IStationRepository
{
    Task<Station?> GetAsync(StationId id);

    Task<Station?> FindByRil100Async(Ril100Identifier ril100Identifier);
    
    Task<List<Station>> GetManyAsync(IEnumerable<EvaNumber> evaNumbers);
    
    Task<List<Station>> GetManyAsync(IEnumerable<StationId> stationIds);
    
    void AddRange(IEnumerable<Station> stations);
    Task<List<Station>> FindManyAsync(IEnumerable<StationName> names);
}
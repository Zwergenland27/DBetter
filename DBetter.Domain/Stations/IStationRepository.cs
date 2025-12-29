using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public interface IStationRepository
{
    Task<Station> GetAsync(StationId id);
}
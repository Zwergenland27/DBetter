using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public interface IStationInfoProvider
{
    Task<StationInformation> GetStationInfoAsync(EvaNumber evaNumber);
}
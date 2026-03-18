using DBetter.Application.Stations.Dtos;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Stations;

public interface IExternalStationProvider
{
    Task<StationProviderDto> GetStationInfoAsync(EvaNumber evaNumber);
    
    Task<List<StationQueryDto>> FindAsync(string query);
}
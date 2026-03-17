using DBetter.Application.Stations.ScrapeDepartures;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Stations;

public interface IExternalDepartureProvider
{
    Task<List<DepartureDto>> GetDepartures(EvaNumber evaNumber, DateOnly forDay);
}
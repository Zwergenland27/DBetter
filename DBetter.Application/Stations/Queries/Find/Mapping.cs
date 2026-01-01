using DBetter.Contracts.Stations.Queries.Find;
using DBetter.Domain.Stations;

namespace DBetter.Application.Stations.Queries.Find;

public static class Mapping
{
    public static StationDto ToDto(this Station station)
    {
        return new StationDto
        {
            Id = station.Id.Value.ToString(),
            Name = station.Name.NormalizedValue,
            Ril100 = station.Ril100?.Value
        };
    }
}
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.Snapshots;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.Stations;

public class Haltestelle
{
    /// <summary>
    /// Eva number
    /// </summary>
    /// <remarks>Only set when place is a station</remarks>
    /// <example>8010085</example>
    public required string ExtId { get; set; }
    
    /// <summary>
    /// Name of the place
    /// </summary>
    /// <example>Dresden Hbf</example>
    public required string Name { get; set; }
    
    /// <summary>
    /// Latitude of the place
    /// </summary>
    /// <example>51.040222</example>
    public required float Lat  { get; set; }
    
    /// <summary>
    /// Longitude of the place
    /// </summary>
    /// <example>13.731409</example>
    public required float Lon { get; set; }
    
    public StationQuerySnapshot? ToSnapshot()
    {
        var evaNumber = EvaNumber.Create(ExtId);
        if (evaNumber.HasFailed) return null;

        var name = StationName.Create(Name);
        if (name.HasFailed) return null;

        var location = new Coordinates(Lat, Lon);

        return new StationQuerySnapshot(
            evaNumber.Value,
            name.Value,
            location);
    }
}
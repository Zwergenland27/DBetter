namespace DBetter.Contracts.Stations.Queries.Find;

public class StationDto
{
    /// <summary>
    /// Internal id of the station
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// Name of the station
    /// </summary>
    /// <example>Dresden Hbf</example>
    public required string Name { get; set; }
}
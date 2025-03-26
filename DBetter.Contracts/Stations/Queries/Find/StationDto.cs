namespace DBetter.Contracts.Stations.Queries.Find;

public class StationDto
{
    //TODO: Do not return evaNumber to the user. Instead always use custom generated StationId.
    
    /// <summary>
    /// EVA number of the station
    /// </summary>
    /// <remarks>Also known as ibnr number</remarks>
    /// <example>8010085</example>
    public required string EvaNumber { get; set; }
    
    /// <summary>
    /// Name of the station
    /// </summary>
    /// <example>Dresden Hbf</example>
    public required string Name { get; set; }
}
namespace DBetter.Contracts.Requests.CreateRequest;

public class ConnectionRouteStopoverDto
{
    /// <summary>
    /// Internal id of stopover station
    /// </summary>
    public string? StationId { get; set; }
    
    /// <summary>
    /// Length of stay at the station in minutes
    /// </summary>
    public int? LengthOfStay { get; set; }
    
    /// <summary>
    /// Means of transport for the next section
    /// </summary>
    public MeansOfTransportDto? MeansOfTransportNextSection { get; set; }
}
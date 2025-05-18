using CleanDomainValidation.Application;

namespace DBetter.Contracts.Connections.Queries.CreateRequest.Parameters;

public class ConnectionRouteStopoverParameters : IParameters
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
    public MeansOfTransportParameters? MeansOfTransportNextSection { get; set; }
}
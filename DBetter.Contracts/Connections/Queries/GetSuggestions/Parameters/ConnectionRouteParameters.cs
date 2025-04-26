using CleanDomainValidation.Application;

namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Parameters;

public class ConnectionRouteParameters : IParameters
{
    /// <summary>
    /// Internal id of origin station
    /// </summary>
    public string? OriginStationId { get; set; }
    
    /// <summary>
    /// Allowed means of transport for next section
    /// </summary>
    public MeansOfTransportParameters? MeansOfTransportFirstSection { get; set; }
    
    /// <summary>
    /// First stopover
    /// </summary>
    public ConnectionRouteStopoverParameters? FirstStopover { get; set; }
    
    /// <summary>
    /// Second stopover
    /// </summary>
    public ConnectionRouteStopoverParameters? SecondStopover { get; set; }
    
    /// <summary>
    /// Internal id of destination station
    /// </summary>
    public string? DestinationStationId { get; set; }
    
    /// <summary>
    /// Maximum number of transfers on the connection
    /// </summary>
    /// <example>5</example>
    public int? MaxTransfers { get; set; }

    /// <summary>
    /// Minimum transfer time for every transfer in minutes
    /// </summary>
    /// <example>10</example>
    public int? MinTransferTime { get; set; }
}
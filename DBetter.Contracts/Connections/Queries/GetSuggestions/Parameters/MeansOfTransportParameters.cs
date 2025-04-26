using CleanDomainValidation.Application;

namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Parameters;

public class MeansOfTransportParameters : IParameters
{
    /// <summary>
    /// Indicates that high speed trains are allowed on this section
    /// </summary>
    /// <remarks>ICE, TGV, RJ, ...</remarks>
    public bool? HighSpeedTrains { get; set; }
    
    /// <summary>
    /// Indicates that fast trains are allowed on this section
    /// </summary>
    /// <remarks>IC, EC, FLX, ...</remarks>
    public bool? FastTrains { get; set; }
    
    /// <summary>
    /// Indicates that regional trains are allowed on this section
    /// </summary>
    /// <remarks>RE, RB, IRE</remarks>
    public bool? RegionalTrains { get; set; }
    
    /// <summary>
    /// Indicates that suburban trains are allowed on this section
    /// </summary>
    /// <remarks>S-Bahn</remarks>
    public bool? SuburbanTrains { get; set; }
    
    /// <summary>
    /// Indicates that underground trains are allowed on this section
    /// </summary>
    /// <remarks>U-Bahn</remarks>
    public bool? UndergroundTrains { get; set; }
    
    /// <summary>
    /// Indicates that trams are allowed on this section
    /// </summary>
    public bool? Trams { get; set; }
    
    /// <summary>
    /// Indicates that busses are allowed on this section
    /// </summary>
    public bool? Busses { get; set; }
    
    /// <summary>
    /// Indicates that boats are allowed on this section
    /// </summary>
    public bool? Boats { get; set; }
}
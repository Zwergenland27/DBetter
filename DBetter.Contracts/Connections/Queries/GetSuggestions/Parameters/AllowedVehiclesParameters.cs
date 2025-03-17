using CleanDomainValidation.Application;

namespace DBetter.Contracts.ConnectionRequests.Commands.Put;

public class AllowedVehiclesParameters : IParameters
{
    /// <summary>
    /// Indicates that high speed trains are allowed on the section
    /// </summary>
    public bool? HighSpeed { get; set; }
    
    /// <summary>
    /// Indicates that intercity like trains are allowed on the section
    /// </summary>
    public bool? Intercity { get; set; }
    
    /// <summary>
    /// Indicates that regional trains are allowed on the section
    /// </summary>
    /// <remarks>
    /// This contains REs and RBs
    /// </remarks>
    public bool? Regional { get; set; }
    
    /// <summary>
    /// Indicates that public transportation is allowed on the section
    /// </summary>
    /// <remarks>
    /// This contains S-Bahnen, Trams, Buses and Ferries.
    /// </remarks>
    public bool? PublicTransport { get; set; }
}
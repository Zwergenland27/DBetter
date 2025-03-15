using CleanDomainValidation.Application;

namespace DBetter.Contracts.ConnectionRequests.Commands.Put;

public class ConnectionRouteParameters : IParameters
{
    /// <summary>
    /// List of the stops eva numbers 
    /// </summary>
    public List<string>? Stops { get; set; }
    
    /// <summary>
    /// Allowed vehicle for every section
    /// </summary>
    public List<AllowedVehiclesParameters>? AllowedVehicles { get; set; }
}
namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Information about the route of the transport product
/// </summary>
public class RouteInformationDto
{
    /// <summary>
    /// If of the train run
    /// </summary>
    public required string RouteId { get; set; }
    
    /// <summary>
    /// Destination if available
    /// </summary>
    /// <example>Görlitz</example>
    public required string? Destination { get; set; }
    
    /// <summary>
    /// The transport product
    /// </summary>
    /// <example>RegionalExpress</example>
    public required string Product { get; set; }
    
    /// <summary>
    /// Line number
    /// </summary>
    /// <remarks>
    /// For long distance trains the train number will be used
    /// </remarks>
    /// <example>2</example>
    public required string Number { get; set; }
    
    /// <summary>
    /// Information about bike carriage
    /// </summary>
    public required BikeCarriageInformationDto BikeCarriage { get; set; }
    
    /// <summary>
    /// Information about catering in the vehicle
    /// </summary>
    public required CateringInformationDto Catering { get; set; }
}
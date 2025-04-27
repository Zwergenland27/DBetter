using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Segment where a mean of transport is used
/// </summary>
public class TransportSegmentDto : SegmentDto
{
    
    /// <summary>
    /// Id of the complete train run
    /// </summary>
    public required string RouteId {get;set;}
    
    /// <summary>
    /// Demand information
    /// </summary>
    public required DemandDto Demand { get; set; }
    
    /// <summary>
    /// Stops of the section of the train run
    /// </summary>
    public required List<StopDto> Stops { get; set; }
    
    /// <summary>
    /// Operator of the service
    /// </summary>
    /// <example>DB Regio Südost</example>
    public required string? Operator { get; set; } 

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
    /// Indicates that this is a replacement service
    /// </summary>
    public required bool ReplacementService { get; set; }
    
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
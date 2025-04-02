namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Segment where a mean of transport is used
/// </summary>
public class TransportSegmentDto : SegmentDto
{
    
    /// <summary>
    /// Id of the complete train run
    /// </summary>
    public required string TrainRunId {get;set;}
    
    /// <summary>
    /// Demand information
    /// </summary>
    public required DemandDto DemandDto { get; set; }
    
    /// <summary>
    /// Stops of the section of the train run
    /// </summary>
    public required List<Stop> Stops { get; set; }
    
    /// <summary>
    /// Route information
    /// </summary>
    public required RouteInformationDto RouteInformationDto { get; set; }
}
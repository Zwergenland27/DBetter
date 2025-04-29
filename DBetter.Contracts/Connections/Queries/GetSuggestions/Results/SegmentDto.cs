using System.Text.Json.Serialization;
using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Segment of a connection
/// </summary>
[JsonDerivedType(typeof(WalkingSegmentDto), typeDiscriminator: "walking")]
[JsonDerivedType(typeof(TransportSegmentDto), typeDiscriminator: "transport")]
public class SegmentDto
{
    /// <summary>
    /// Departure time of this section
    /// </summary>
    public required TravelTimeDto DepartureTime { get; set; }
    
    /// <summary>
    /// Arrival time of this section
    /// </summary>
    public required TravelTimeDto ArrivalTime { get; set; }
}
using System.Text.Json.Serialization;

namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Segment of a connection
/// </summary>
[JsonDerivedType(typeof(WalkingSegmentDto), typeDiscriminator: "walking")]
[JsonDerivedType(typeof(TransportSegmentDto), typeDiscriminator: "transport")]
public class SegmentDto
{

}
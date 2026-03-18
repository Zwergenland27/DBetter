using System.Text.Json.Serialization;

namespace DBetter.Contracts.Requests.Queries.GetSuggestions.Results;

/// <summary>
/// Segment of a connection
/// </summary>
[JsonDerivedType(typeof(TransferSegmentResponse), typeDiscriminator: "transfer")]
[JsonDerivedType(typeof(TransportSegmentResponse), typeDiscriminator: "transport")]
[JsonDerivedType(typeof(WalkingSegmentResponse), typeDiscriminator: "walking")]
public class SegmentResponse;
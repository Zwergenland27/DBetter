using System.Text.Json.Serialization;
using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Segment of a connection
/// </summary>
[JsonDerivedType(typeof(TransferSegmentDto), typeDiscriminator: "transfer")]
[JsonDerivedType(typeof(TransportSegmentDto), typeDiscriminator: "transport")]
[JsonDerivedType(typeof(WalkingSegmentDto), typeDiscriminator: "walking")]
public class SegmentDto;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Requests.IncreaseTransferTime;

public record FixedSubConnection
{
    public required EvaNumber StartEvaNumber { get; init; }
    
    public required DateTime StartTime { get; init; }
    
    public required EvaNumber EndEvaNumber { get; init; }
    
    public required DateTime EndTime { get; init; }
    
    public required MeansOfTransport MeansOfTransport { get; init; }
}
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record Stopover(StationId StationId, int LengthOfStay, MeansOfTransport MeansOfTransportNextSection)
{
    private Stopover() : this(null!, 0, null!)
    {}
}
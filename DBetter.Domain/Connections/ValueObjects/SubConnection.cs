using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Connections.ValueObjects;

public record SubConnection
{
    public StationId FromStationId { get; private init; }
    public DateTime DepartureTime { get; private init; }
    public StationId ToStationId { get; private init; }
    public DateTime ArrivalTime { get; private init; }
    public MeansOfTransport OriginalMeansOfTransport { get; private init; }
    
    private SubConnection(){}

    public SubConnection(
        StationId fromStationId,
        DateTime departureTime,
        StationId toStationId,
        DateTime arrivalTime,
        MeansOfTransport originalMeansOfTransport)
    {
        FromStationId = fromStationId;
        DepartureTime = departureTime;
        ToStationId = toStationId;
        ArrivalTime = arrivalTime;
        OriginalMeansOfTransport = originalMeansOfTransport;
    }
}
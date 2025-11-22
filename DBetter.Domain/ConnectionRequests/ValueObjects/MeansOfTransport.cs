namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record MeansOfTransport(
    bool HighSpeedTrains,
    bool FastTrains,
    bool RegionalTrains,
    bool SuburbanTrains,
    bool UndergroundTrains,
    bool Trams,
    bool Busses,
    bool Boats)
{
    public bool AnySelected => HighSpeedTrains || FastTrains || RegionalTrains || SuburbanTrains || UndergroundTrains ||
                               Trams || Busses || Boats;
}
using DBetter.Application.Shared;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Requests.Snapshots;

public record ConnectionSnapshot
{
    public required ConnectionContextId ContextId { get; init; }
    public required Demand Demand { get; init; }
    
    public required List<SegmentSnapshot> Segments { get; init; }
    
    public required Offer? Offer { get; init; }
    
    public required BookingLink BookingLink { get; init; }
    
    public List<BahnJourneyId> JourneyIds => Segments
        .OfType<TransportSegmentSnapshot>() 
        .Select(s => s.JourneyId)
        .Distinct()
        .ToList();
    
    public List<EvaNumber> StationEvaNumbers => Segments
        .OfType<TransportSegmentSnapshot>()
        .SelectMany(s => s.Stops)
        .Select(stop => stop.EvaNumber)
        .Distinct()
        .ToList();

    public List<StopSnapshot> GetUnknownStations(List<Station> existingStations)
    {
        return Segments
            .OfType<TransportSegmentSnapshot>()
            .SelectMany(s => s.Stops)
            .Where(s => existingStations.All(es => es.EvaNumber != s.EvaNumber))
            .ToList();
    }
    
    public List<PassengerInformationDto> PassengerInformation => Segments
        .OfType<TransportSegmentSnapshot>()
        .SelectMany(s => s.PassengerInformation)
        .Distinct()
        .ToList();
}
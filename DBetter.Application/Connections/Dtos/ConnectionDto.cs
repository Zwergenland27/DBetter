using DBetter.Application.Requests.Dtos;
using DBetter.Application.Shared;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;

namespace DBetter.Application.Connections.Dtos;

public record ConnectionDto
{
    public required ConnectionContextId ContextId { get; init; }
    public required Demand Demand { get; init; }
    
    public required List<SegmentDto> Segments { get; init; }
    
    public required Offer? Offer { get; init; }
    
    public required BookingLink BookingLink { get; init; }
    
    public List<BahnJourneyId> JourneyIds => Segments
        .OfType<TransportSegmentDto>() 
        .Select(s => s.JourneyId)
        .Distinct()
        .ToList();
    
    public List<EvaNumber> StationEvaNumbers => Segments
        .OfType<TransportSegmentDto>()
        .SelectMany(s => s.Stops)
        .Select(stop => stop.EvaNumber)
        .Distinct()
        .ToList();

    public List<StopDto> GetUnknownStations(List<Station> existingStations)
    {
        return Segments
            .OfType<TransportSegmentDto>()
            .SelectMany(s => s.Stops)
            .Where(s => existingStations.All(es => es.EvaNumber != s.EvaNumber))
            .ToList();
    }
    
    public List<PassengerInformationDto> PassengerInformation => Segments
        .OfType<TransportSegmentDto>()
        .SelectMany(s => s.PassengerInformation)
        .Distinct()
        .ToList();
}
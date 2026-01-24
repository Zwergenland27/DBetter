using DBetter.Application.Shared;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Requests.Snapshots;

public record TransportSegmentSnapshot : SegmentSnapshot
{
    public required BahnJourneyId JourneyId { get; init; }
    
    public required Demand Demand { get; init; }
    
    public required List<StopSnapshot> Stops { get; init; }
    
    public required StationName? Destination { get; init; }
    
    public required List<ServiceInformation> Composition { get; init; }
    
    public required BikeCarriageInformation BikeCarriage { get; init; }
    
    public required CateringInformation Catering { get; init; }
    
    public required List<PassengerInformationDto> PassengerInformation { get; init; }
}
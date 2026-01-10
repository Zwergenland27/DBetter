using DBetter.Domain.Connections.Snapshots;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Routes.Snapshots;

public class RouteSnapshot
{
    public required List<StopSnapshot> Stops { get; init; }
    
    public required List<ServiceNumber> ServiceNumbers { get; init; }
    
    public required BikeCarriageInformation BikeCarriage { get; init; }
    
    public required CateringInformation Catering { get; init; }
    
    public required List<PassengerInformation> InformationMessages { get; init; }

    public List<StopSnapshot> GetUnknownStations(List<Station> existingStations)
    {
        return Stops
            .Where(s => existingStations.All(es => es.EvaNumber != s.EvaNumber))
            .ToList();
    }
}
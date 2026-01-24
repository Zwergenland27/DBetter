using DBetter.Application.Requests.Snapshots;
using DBetter.Application.Shared;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations;

namespace DBetter.Application.Routes.Dtos;

public class RouteSnapshot
{
    public required List<StopSnapshot> Stops { get; init; }
    
    public required List<ServiceNumber> ServiceNumbers { get; init; }
    
    public required BikeCarriageInformation BikeCarriage { get; init; }
    
    public required CateringInformation Catering { get; init; }
    
    public required List<PassengerInformationDto> PassengerInformation { get; init; }
    public List<StopSnapshot> GetUnknownStations(List<Station> existingStations)
    {
        return Stops
            .Where(s => existingStations.All(es => es.EvaNumber != s.EvaNumber))
            .ToList();
    }
    
    public List<PassengerInformationDto> GetUnknownPassengerInformation(List<PassengerInformation> existingPassengerInformation)
    {
        return PassengerInformation
            .Where(dto => existingPassengerInformation.All(im => im.Text != dto.OriginalText))
            .ToList();
    }
}
using DBetter.Application.Requests.Dtos;
using DBetter.Application.Shared;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.TrainRuns.Dtos;

public class TrainRunDto
{
    public required List<StopDto> Stops { get; init; }
    
    public required List<ServiceNumber> ServiceNumbers { get; init; }
    
    public required BikeCarriageInformation BikeCarriage { get; init; }
    
    public required CateringInformation Catering { get; init; }
    
    public required List<PassengerInformationDto> PassengerInformation { get; init; }
    public List<StopDto> GetUnknownStations(List<Station> existingStations)
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
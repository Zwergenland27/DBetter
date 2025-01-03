namespace DBetter.Contracts.Journeys.DTOs;

public class ConnectionSectionDto
{
    public string LineNr { get; set; }
    public List<VehicleDto>? Vehicle { get; set; }
    public float Percentage { get; set; }
    public string Catering { get; set; }
    public string Bike { get; set; }
    public string Accessibility { get; set; }
    public DemandDto Demand { get; set; }
    public List<InformationDto> Information { get; set; }
    public List<ConnectionStationDto> Stops { get; set; }
}
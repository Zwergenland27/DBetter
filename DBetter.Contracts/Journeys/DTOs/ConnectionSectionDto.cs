namespace DBetter.Contracts.Journeys.DTOs;

public class ConnectionSectionDto
{
    public string LineNameShort { get; set; }
    
    public string LineNameMedium { get; set; }
    
    public string LineNameFull { get; set; }
    
    public string Category { get; set; }
    
    public string LineNumber { get; set; }
    
    public string Direction { get; set; }
    public float Percentage { get; set; }
    public string Catering { get; set; }
    public string Bike { get; set; }
    public string Accessibility { get; set; }
    public DemandDto Demand { get; set; }
    public List<InformationDto> Information { get; set; }
    
    public bool ReservationRequired { get; set; }
    public List<ConnectionStationDto> Stops { get; set; }
    
    public string? ConnectionPrediction { get; set; }
    
    public string JourneyId { get; set; }
}
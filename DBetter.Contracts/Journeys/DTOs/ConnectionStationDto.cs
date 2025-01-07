namespace DBetter.Contracts.Journeys.DTOs;

public class ConnectionStationDto : StationDto
{
    public int RouteIndex { get; set; }
    public DateTime? Arrival { get; set; }
    
    public DateTime? RealTimeArrival { get; set; }
    public DateTime? Departure { get; set; }
    
    public DateTime? RealTimeDeparture { get; set; }
    
    public DemandDto Demand { get; set; }
    
    public string Platform { get; set; }
    
    public List<InformationDto> Information { get; set; }
    
    public string ExternalId { get; set; }
}
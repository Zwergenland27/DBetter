namespace DBetter.Contracts.Journeys.DTOs;

public class ConnectionsDto
{
    public List<ConnectionDto> Connections { get; set; }
    
    public string PageEarlier { get; set; }
    
    public string PageLater { get; set; }
}
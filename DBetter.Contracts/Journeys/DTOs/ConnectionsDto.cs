namespace DBetter.Contracts.Journeys.DTOs;

public class ConnectionsDto
{
    public DateTime Test { get; set; } = DateTime.UtcNow;
    public List<ConnectionDto> Connections { get; set; }
    
    public string PageEarlier { get; set; }
    
    public string PageLater { get; set; }
}
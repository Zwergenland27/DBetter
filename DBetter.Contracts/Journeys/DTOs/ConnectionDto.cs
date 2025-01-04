namespace DBetter.Contracts.Journeys.DTOs;

public class ConnectionDto
{
    public string Id { get; set; }
    public List<ConnectionSectionDto> Sections { get; set; }
    public PriceDto? Price { get; set; }
    public List<InformationDto> Information { get; set; }
    
    public string? Bike { get; set; }
    
    public DemandDto Demand { get; set; }
    
}
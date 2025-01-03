namespace DBetter.Contracts.Journeys.DTOs;

public class InformationDto
{
    public int Priority { get; set; }
    
    public string Code { get; set; }
    
    public int? RouteIndexStart { get; set; }
    
    public int? RouteIndexEnd { get; set; }
}
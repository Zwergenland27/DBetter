namespace DBetter.Infrastructure.ApiMarketplace.StaDa.DTOs;

public class EvaNumber
{
    public required GeographicCoordinates GeographicCoordinates { get; set; }
    
    public required bool IsMain { get; set; }
}
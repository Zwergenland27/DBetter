namespace DBetter.Infrastructure.BahnDe.Stations;

public class StationDto
{
    public string ExtId { get; set; }
    
    public string Name { get; set; }
    
    public float Lat  { get; set; }
    
    public float Lon { get; set; }
    
    public string Type { get; set; }
}
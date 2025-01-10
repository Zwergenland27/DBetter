using System.Text.Json.Serialization;

namespace DBetter.Contracts;

public class StationDto
{
    public String Id { get; set; } = null!;

    public String Name { get; set; } = null!;
    
    public string ExtId { get; set; }
}
namespace DBetter.Infrastructure.BahnApi.Journey.Parameters;

public class Zwischenhalt
{
    public int? Aufenthaltsdauer {get; set;}
    
    public string Id { get; set; }
    
    public List<string> VerkehrsmittelOfNextAbschnitt { get; set; }
}
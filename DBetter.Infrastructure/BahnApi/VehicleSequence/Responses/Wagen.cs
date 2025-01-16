namespace DBetter.Infrastructure.BahnApi.VehicleSequence.Responses;

public class Wagen
{
    public List<Platz> Plaetze { get; set; }
    
    public string Nummer  { get; set; }
    
    public string Wagentyp { get; set; }
}
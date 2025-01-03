namespace DBetter.Infrastructure.BahnApi.Journey.Responses;

public class Verkehrsmittel
{
    public string MittelText { get; set; }
    
    public string ProduktGattung { get; set; }
    
    public List<ZugAttribut> ZugAttribute { get; set; }
}
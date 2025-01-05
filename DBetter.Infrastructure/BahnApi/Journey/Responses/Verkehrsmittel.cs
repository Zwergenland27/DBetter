namespace DBetter.Infrastructure.BahnApi.Journey.Responses;

public class Verkehrsmittel
{
    public string KurzText { get; set; }
    public string MittelText { get; set; }
    
    public string LangText { get; set; }
    
    public string ProduktGattung { get; set; }
    
    public string Richtung { get; set; }
    
    public List<ZugAttribut> ZugAttribute { get; set; }
}
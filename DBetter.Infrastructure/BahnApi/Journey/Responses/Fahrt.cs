namespace DBetter.Infrastructure.BahnApi.Journey.Responses;

public class Fahrt
{
    public string Zugname { get; set; }
    public List<HimMeldung> HimMeldungen { get; set; }
    
    public List<RisNotiz> RisNotizen  { get; set; }
    
    public List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
    
    public List<Halt> Halte { get; set; }
    
    public List<ZugAttribut> Zugattribute { get; set; }
    
    public bool Cancelled { get; set; }
}
namespace DBetter.Infrastructure.BahnApi.Journey.Responses;

public class Verbindungsabschnitt
{
    public List<HimMeldung> HimMeldungen { get; set; }
    
    public List<RisNotiz> RisNotizen  { get; set; }
    
    public List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
    
    public float AbschnittsAnteil { get; set; }
    
    public Verkehrsmittel? Verkehrsmittel { get; set; }
    
    public string ReservierungspflichtigNote { get; set; }
    
    public List<Halt> Halte { get; set; }
    
    public List<AuslastungsMeldung> AuslastungsMeldungen { get; set; }
}
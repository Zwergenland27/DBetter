namespace DBetter.Infrastructure.BahnApi.Journey.Responses;

public class Verbindung
{
    public string TripId { get; set; }
    
    public string CtxRecon  { get; set; }
    
    public List<Verbindungsabschnitt> VerbindungsAbschnitte { get; set; }
    
    public string FahrradmitnahmeMoeglich	 { get; set; }
    
    public PreisAngebot? AngebotsPreis { get; set; }
    
    public List<HimMeldung> HimMeldungen { get; set; }
    
    public List<RisNotiz> RisNotizen  { get; set; }
    
    public List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
}
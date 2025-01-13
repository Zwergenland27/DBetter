namespace DBetter.Infrastructure.BahnApi.Journey.Responses;

public class Verbindung
{
    public string TripId { get; set; }
    
    public string CtxRecon  { get; set; }
    
    public List<Verbindungsabschnitt> VerbindungsAbschnitte { get; set; }
    
    public string FahrradmitnahmeMoeglich	 { get; set; }
    
    public PreisAngebot? AngebotsPreis { get; set; }
    
    public bool HasTeilpreis { get; set; }
    
    public List<HimMeldung> HimMeldungen { get; set; }
    
    public List<RisNotiz> RisNotizen  { get; set; }
    
    public List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
    
    public List<AuslastungsMeldung> AuslastungsMeldungen { get; set; }
    
    public string AngebotsPreisKlasse { get; set; } //KLASSE_1 -> will be displayed if 2nd class is overbooked or KLASSE_2
}
namespace DBetter.Infrastructure.BahnApi.Journey.Responses;

public class Halt
{
    public string Id { get; set; }
    
    public int RouteIdx { get; set; }
    
    public string Name { get; set; }
    
    public string? AbfahrtsZeitpunkt { get; set; }
    
    public string? EzAbfahrtsZeitpunkt { get; set; }
    
    public string? AnkunftsZeitpunkt { get; set; }
    
    public string? EzAnkunftsZeitpunkt { get; set; }
    
    public string Gleis { get; set; }
    
    public string ExtId { get; set; }
    
    public List<HimMeldung> HimMeldungen { get; set; }
    
    public List<RisNotiz> RisNotizen  { get; set; }
    
    public List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
    
    public List<AuslastungsMeldung> AuslastungsMeldungen { get; set; }
}
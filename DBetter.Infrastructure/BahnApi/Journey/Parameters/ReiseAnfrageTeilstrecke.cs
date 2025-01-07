namespace DBetter.Infrastructure.BahnApi.Journey.Parameters;

public class ReiseAnfrageTeilstrecke
{
    public string OriginalCtxRecon { get; set; }
    public string Klasse { get; set; }
    public string AnkunftSuche { get; set; }
    public List<string> Produktgattungen { get; set; }
    public List<Reisender> Reisende { get; set; }
    
    public FixedTeilstrecke FixedTeilstrecke { get; set; }
    
    public int MaxUmstiege { get; set; }
    
    public bool SchnelleVerbindungen { get; set; }
    public bool SitzplatzOnly { get; set; }
    public bool BikeCarriage { get; set; }
    public bool ReservierungsKontingenteVorhanden { get; set; }
    public bool NurDeutschlandTicketVerbindungen { get; set; }
    public bool DeutschlandTicketVorhanden { get; set; }
    public List<Zwischenhalt> Zwischenhalte { get; set; }
}
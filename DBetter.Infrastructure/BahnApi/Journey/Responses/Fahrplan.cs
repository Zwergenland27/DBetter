namespace DBetter.Infrastructure.BahnApi.Journey.Responses;

public class Fahrplan
{
    public List<Verbindung> Verbindungen { get; set; }
    
    public VerbindungReference VerbindungReference { get; set; }
}
namespace DBetter.Infrastructure.BahnApi.Journey.Responses;

public class PreisAngebot
{
    public float Betrag { get; set; }
    
    public string Waehrung { get; set; }
    
    public bool HasTeilPreis { get; set; }
}
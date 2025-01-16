namespace DBetter.Infrastructure.BahnApi.VehicleSequence.Parameters;

public class BuchungsKontextDaten
{
    public string Zugnummer { get; set; }
    public StartHalt AbfahrtHalt { get; set; }
    public EndHalt AnkunftHalt { get; set; }
    public string ServicekategorieCode { get; set; } = "KLASSE_2";
    public int AnzahlReisende { get; set; } = 1;
    public string Klasse { get; set; } = "KLASSE_2";
}
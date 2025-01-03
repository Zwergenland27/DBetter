namespace DBetter.Infrastructure.BahnApi.Journey.Parameters;

public class Reisender
{
    public List<int> Alter { get; set; }
    public int Anzahl { get; set; }
    public List<Ermaessigung> Ermaessigungen { get; set; }
    public string Typ { get; set; }
}
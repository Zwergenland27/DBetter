namespace DBetter.Contracts.Journeys.Parameters;

public class DiscountParameters
{
    public string Type { get; set; }
    public string Class { get; set; }
    public DateTime? ValidUntil { get; set; }
}
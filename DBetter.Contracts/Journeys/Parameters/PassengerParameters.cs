namespace DBetter.Contracts.Journeys.Parameters;

public class PassengerParameters
{
    public string Id { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public string? Birthday { get; set; }
    public int? Age { get; set; }
    public bool WithSeat { get; set; }
    public int Bikes { get; set; }
    public int Dogs { get; set; }
    public bool WithBuggy { get; set; }
    public bool NeedsAccessibility { get; set; }
    public List<DiscountParameters> Discounts { get; set; }
}
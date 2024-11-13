namespace DBetter.Contracts.Users.Queries.GetMyPassengers;

public class DiscountResult
{
    public required string Type { get; set; }
    
    public required string Class { get; set; }
    
    public required DateTime? ValidUntil { get; set; }
}
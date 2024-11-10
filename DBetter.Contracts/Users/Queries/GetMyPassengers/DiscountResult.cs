namespace DBetter.Contracts.Users.Queries.GetMyPassengers;

public class DiscountResult
{
    public string Type { get; set; }
    
    public string Class { get; set; }
    
    public DateTime? ValidUntil { get; set; }
}
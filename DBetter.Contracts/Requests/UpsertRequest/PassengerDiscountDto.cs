namespace DBetter.Contracts.Requests.CreateRequest;

public class PassengerDiscountDto
{
    /// <summary>
    /// Type of the Discount
    /// </summary>
    /// <example>BahnCard25</example>
    public string? Type { get; set; }
    
    /// <summary>
    /// Class where the discount is valid
    /// </summary>
    /// <example>Second</example>
    public string? ComfortClass { get; set; }
}
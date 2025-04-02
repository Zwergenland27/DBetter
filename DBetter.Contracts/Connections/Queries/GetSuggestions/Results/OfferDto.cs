namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Offer for a connection
/// </summary>
public class OfferDto
{
    /// <summary>
    /// Comfort class of the price offer
    /// </summary>
    public required string ComfortClass { get; set; }
    
    /// <summary>
    /// Price of the connection
    /// </summary>
    public required float Price { get; set; }
    
    /// <summary>
    /// Currency of the <see cref="Price"/>
    /// </summary>
    public required string Currency { get; set; }
    
    /// <summary>
    /// Indicates wether the offer is only for a section of the connection
    /// </summary>
    public required bool Partial { get; set; }
}
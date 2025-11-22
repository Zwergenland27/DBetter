namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Price offer
/// </summary>
public class AngebotsPreis
{
    /// <summary>
    /// Offer price
    /// </summary>
    public required float Betrag { get; set; }
    
    /// <summary>
    /// Currency of the price
    /// </summary>
    public required Waehrung Waehrung { get; set; }
}
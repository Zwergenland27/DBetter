namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Price offer
/// </summary>
public class AngebotsPreis
{
    /// <summary>
    /// Offer price
    /// </summary>
    public float Betrag { get; set; }
    
    /// <summary>
    /// Currency of the price
    /// </summary>
    public Waehrung Waehrung { get; set; }
}
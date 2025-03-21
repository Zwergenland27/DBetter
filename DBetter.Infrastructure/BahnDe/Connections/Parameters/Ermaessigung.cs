namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Discount
/// </summary>
public class Ermaessigung
{
    /// <summary>
    /// Discount type
    /// </summary>
    public required ArtErmaessigung Art { get; set; }
    
    /// <summary>
    /// Comfort class where the discount is valid
    /// </summary>
    public required KlasseErmaessigung Klasse { get; set; }

    public static Ermaessigung Keine()
    {
        return new Ermaessigung
        {
            Art = ArtErmaessigung.KEINE_ERMAESSIGUNG,
            Klasse = KlasseErmaessigung.KLASSENLOS
        };
    }
}
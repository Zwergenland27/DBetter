using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Discount
/// </summary>
public class Ermaessigung
{
    /// <summary>
    /// Discount type
    /// </summary>
    /// <remarks>Use <see cref="ArtErmaessigung"/> to serialize / deserialize this string</remarks>
    public required string Art { get; set; }
    
    /// <summary>
    /// Comfort class where the discount is valid
    /// </summary>
    /// <remarks>Use <see cref="KlasseErmaessigung"/> to serialize / deserialize this string</remarks>
    public required string Klasse { get; set; }

    public static Ermaessigung None()
    {
        return new Ermaessigung
        {
            Art = ArtErmaessigung.None,
            Klasse = KlasseErmaessigung.GetAliasFromComfortClass(ComfortClass.Unknown)
        };
    }

    public static Ermaessigung Create(PassengerDiscount discount)
    {
        return new Ermaessigung
        {
            Art = ArtErmaessigung.GetAliasFromType(discount.Type),
            Klasse = KlasseErmaessigung.GetAliasFromComfortClass(discount.ComfortClass)
        };
    }
}
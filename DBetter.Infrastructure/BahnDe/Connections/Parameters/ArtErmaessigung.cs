using System.Diagnostics.CodeAnalysis;
using DBetter.Domain.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Type of discount
/// </summary>
public static class ArtErmaessigung
{
    private record DiscountWithAlias(string Alias, DiscountType Type, string UrlId);
    
    private static List<DiscountWithAlias> _discountTypes =
    [
        new("BAHNCARD25", DiscountType.BahnCard25, "17"),
        new("BAHNCARDBUSINESS25", DiscountType.BahnCard25Business, "19"),
        new("BAHNCARD50", DiscountType.BahnCard50, "23"),
        new("BAHNCARDBUSINESS50", DiscountType.BahnCard50Business, "18"),
        new("BAHNCARD100", DiscountType.BahnCard100, "24"),
        new("CH-GENERAL-ABONNEMENT", DiscountType.CHGeneralAbonnement, "1"),
        new("CH-HALBTAXABO_OHNE_RAILPLUS", DiscountType.HalbtaxAbo, "21"),
        new("A-VORTEILSCARD", DiscountType.VorteilsCardAu, "20"),
        new("NL-40_OHNE_RAILPLUS", DiscountType.Nl40, "22"),
        new("KLIMATICKET_OE", DiscountType.KlimaTicketAu, "7")
    ];
    
    /// <summary>
    /// No discount
    /// </summary>
    public static string None => "KEINE_ERMAESSIGUNG";

    public static string NoneId => "16";

    public static DiscountType GetTypeFromAlias(string alias)
    {
        var result = _discountTypes.FirstOrDefault(x => x.Alias == alias);

        if (result is null)
        {
            throw new BahnDeException("Mapping.ArtErmaessigung", $"Alias {alias} not found");
        }
        
        return result.Type;
    }

    public static string GetAliasFromType(DiscountType discountType)
    {
        var result = _discountTypes.FirstOrDefault(x => x.Type == discountType);

        if (result is null)
        {
            throw new BahnDeException("Mapping.ArtErmaessigung", $"Discount type {discountType} not found");
        }
        
        return result.Alias;
    }


    public static string GetUrlIdFromAlias(string alias)
    {
        if (alias == None) return NoneId;
        
        var result = _discountTypes.FirstOrDefault(x => x.Alias == alias);

        if (result is null)
        {
            throw new BahnDeException("Mapping.ArtErmaessigung", $"Alias {alias} not found");
        }
        
        return result.UrlId;
    }
}
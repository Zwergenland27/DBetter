using DBetter.Domain.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Comfort class for discounts
/// </summary>
public static class KlasseErmaessigung
{
    private record ComfortClassWithAlias(string Alias, ComfortClass ComfortClass);
    private static readonly ComfortClassWithAlias[] ComfortClasses =
    [
        new("KLASSE_1", ComfortClass.First),
        new("KLASSE_2", ComfortClass.Second),
        new("KLASSENLOS", ComfortClass.Unknown)
    ];
    
    public static ComfortClass GetComfortClassFromAlias(string alias)
    {
        var result = ComfortClasses.FirstOrDefault(x => x.Alias == alias);

        if (result is null)
        {
            throw new BahnDeException("Mapping.KlasseErmaessigung", $"Alias {alias} not found");
        }
        
        return result.ComfortClass;
    }

    public static string GetAliasFromComfortClass(ComfortClass comfortClass)
    {
        var result = ComfortClasses.FirstOrDefault(x => x.ComfortClass == comfortClass);

        if (result is null)
        {
            throw new BahnDeException("Mapping.KlasseErmaessigung", $"Comfort class {comfortClass} not found");
        }
        
        return result.Alias;
    }
}
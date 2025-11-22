using DBetter.Domain.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Comfort class
/// </summary>
public static class Klasse
{
    private record ComfortClassWithAlias(string Alias, ComfortClass ComfortClass, string UrlAlias);

    private static readonly ComfortClassWithAlias[] ComfortClasses =
    [
        new ("KLASSE_1", ComfortClass.First, "1"),
        new ("KLASSE_2", ComfortClass.Second, "2")
    ];
    
    public static ComfortClass GetComfortClassFromAlias(string alias)
    {
        var result = ComfortClasses.FirstOrDefault(x => x.Alias == alias);

        if (result is null)
        {
            throw new BahnDeException("Mapping.Klasse", $"Alias {alias} not found");
        }
        
        return result.ComfortClass;
    }

    public static string GetAliasFromComfortClass(ComfortClass comfortClass)
    {
        var result = ComfortClasses.FirstOrDefault(x => x.ComfortClass == comfortClass);

        if (result is null)
        {
            throw new BahnDeException("Mapping.Klasse", $"Comfort class {comfortClass} not found");
        }
        
        return result.Alias;
    }


    public static string GetUrlAliasFromAlias(string alias)
    {
        var result = ComfortClasses.FirstOrDefault(x => x.Alias == alias);

        if (result is null)
        {
            throw new BahnDeException("Mapping.Klasse", $"Alias {alias} not found");
        }
        
        return result.UrlAlias;
    }
}
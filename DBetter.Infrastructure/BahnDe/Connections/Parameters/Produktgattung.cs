using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Means of transport
/// </summary>
public static class Produktgattung
{
    private record TransportCategoryWithAlias(string Alias,  TransportCategory Category, string UrlId);
    
    private static readonly TransportCategoryWithAlias[] TransportCategories= [
        new("ICE", TransportCategory.HighSpeedTrain, "00"),
        new("EC_IC", TransportCategory.FastTrain, "01"),
        new("IR", TransportCategory.FastTrain, "02"),
        new("REGIONAL", TransportCategory.RegionalTrain, "03"),
        new("SBAHN", TransportCategory.SuburbanTrain, "04"),
        new("BUS", TransportCategory.Bus, "05"),
        new("SCHIFF", TransportCategory.Boat, "06"),
        new("UBAHN", TransportCategory.UndergroundTrain, "07"),
        new("TRAM", TransportCategory.Tram, "08"),
        new("ANRUFPFLICHTIG", TransportCategory.CallService, "09"),
        new("ERSATZVERKEHR", TransportCategory.Replacement, string.Empty) //There is no option to select/deselect replacement services in the uri -> urlId empty
    ];
    
    public static TransportCategory GetTransportCategoryFromAlias(string alias)
    {
        var result = TransportCategories.FirstOrDefault(x => x.Alias == alias);

        if (result is null)
        {
            throw new BahnDeException("Mapping.Produktgattung", $"Alias {alias} not found");
        }
        
        return result.Category;
    }

    public static List<string> GetAliasesFromTransportCategory(TransportCategory transportCategory)
    {
        var transportCategories = TransportCategories
            .Where(x => x.Category == transportCategory)
            .Select(x => x.Alias)
            .ToList();

        if (!transportCategories.Any())
            throw new BahnDeException("Mapping.Produktgattung", $"transport category {transportCategory} not found");
        return transportCategories;
    }


    public static List<string> GetUrlIdsFromTransportCategory(TransportCategory transportCategory)
    {
        var transportCategories = TransportCategories
            .Where(x => x.Category == transportCategory)
            .Select(x => x.UrlId)
            .ToList();

        if (!transportCategories.Any())
            throw new BahnDeException("Mapping.Produktgattung", $"transport category {transportCategory} not found");
        return transportCategories;
    }
}
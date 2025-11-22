namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Type of passenger group
/// </summary>
public class ReisenderTyp
{
    private record ReisenderTypWithAlias(string Type, int UrlId);
    private record PersonGroupWithAlias(string Type, int MinAge, int UrlId) : ReisenderTypWithAlias(Type, UrlId);
    
    private static readonly ReisenderTypWithAlias[] Types =
    [
        new ("FAHRRAD", 3),
        new ("HUND", 14),
        new PersonGroupWithAlias("KLEINKIND", 0, 8),
        new PersonGroupWithAlias("FAMILIENKIND", 6, 11),
        new PersonGroupWithAlias("JUGENDLICHER", 15, 9),
        new PersonGroupWithAlias("ERWACHSENER", 27, 64),
        new PersonGroupWithAlias("SENIOR", 65, 12)
    ];

    private static ReisenderTypWithAlias Bike => Types[0];
    
    private static ReisenderTypWithAlias Dog => Types[1];
    
    public static string GetTypeFromAge(int age)
    {
        return Types
            .OfType<PersonGroupWithAlias>()
            .OrderBy(pg => pg.MinAge)
            .Last(pg => age > pg.MinAge)
            .Type;
    }

    public static int GetUrlIdFromType(string type)
    {
        return Types
            .First(t => t.Type == type)
            .UrlId;
    }
    
    public static string GetBikeAlias() => Bike.Type;
    
    public static string GetDogAlias() => Dog.Type;
}
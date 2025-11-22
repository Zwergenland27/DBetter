
namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Passenger
/// </summary>
/// <remarks>
/// In this context a passenger is not a single person but a group of passengers with the same attributes (agegroup, discounts, ...)
/// </remarks>
public class Reisender
{
    /// <summary>
    /// Age of the passenger(s)
    /// </summary>
    /// <remarks>
    /// Since one object can contain multiple persons of different age, all ages can be set in this array if needed
    /// </remarks>
    public required List<string> Alter { get; set; }
    
    /// <summary>
    /// Number of passengers of this kind
    /// </summary>
    public required int Anzahl { get; set; }
    
    /// <summary>
    /// Possible discounts
    /// </summary>
    public required List<Ermaessigung> Ermaessigungen { get; set; }
    
    /// <summary>
    /// Type of the passenger
    /// </summary>
    /// <remarks>Use <see cref="ReisenderTyp"/> to serialize / deserialize this string</remarks>
    public required string Typ { get; set; }

    public static Reisender Default => new()
    {
        Alter = [],
        Anzahl = 1,
        Ermaessigungen = [Ermaessigung.None()],
        Typ = ReisenderTyp.GetTypeFromAge(30)
    };

    public static Reisender Bikes(int number) => new()
    {
        Alter = [],
        Anzahl = number,
        Ermaessigungen = [Ermaessigung.None()],
        Typ = ReisenderTyp.GetBikeAlias()
    };
    
    public static Reisender Dogs(int number) => new()
    {
        Alter = [],
        Anzahl = number,
        Ermaessigungen = [Ermaessigung.None()],
        Typ = ReisenderTyp.GetDogAlias()
    };
}
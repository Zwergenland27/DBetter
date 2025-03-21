namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Journey Request
/// </summary>
public class ReiseAnfrage
{
    /// <summary>
    /// Fuzzy departure station id
    /// </summary>
    /// <example>@L=8010085</example>
    public required string AbfahrtsHalt { get; set; }
    
    /// <summary>
    /// Fuzzy arrival station id
    /// </summary>
    /// <example>@L=8000105</example>
    public required string AnkunftsHalt { get; set; }
    
    /// <summary>
    /// Departure / Arrival time
    /// </summary>
    /// <remarks>
    /// Date format: yyyy-mm-ddTHH:MM:ss, german time zone
    /// If time should be interpreted as arrival or departure time is dependent on <see cref="AnkunftSuche"/>
    /// </remarks>
    /// <example>2025-03-14T18:01:08</example>
    public required string AnfrageZeitpunkt { get; set; }
    
    /// <summary>
    /// Comfort Class of travel
    /// </summary>
    public required Klasse Klasse { get; set; }
    
    /// <summary>
    /// Sets the <see cref="AnfrageZeitpunkt"/> to departure or arrival
    /// </summary>
    public required AnkunftSuche AnkunftSuche { get; set; }
    
    /// <summary>
    /// Allowed vehicles for the journey
    /// </summary>
    public required List<Produktgattung> Produktgattungen { get; set; }
    
    /// <summary>
    /// Passengers
    /// </summary>
    public required List<Reisender> Reisende { get; set; }
    
    /// <summary>
    /// Maximum number of transfers on the connection
    /// </summary>
    public required int MaxUmstiege { get; set; }
    
    /// <summary>
    /// Minimum transfer time in minutes
    /// </summary>
    public required int MinUmstiegszeit { get; set; }
    
    /// <summary>
    /// Indicate that only fast connections should be suggested
    /// </summary>
    /// <remarks>
    /// I suggest setting this to false, because sometimes a slightly longer connection won't be suggested
    /// even if noticeable cheaper
    /// </remarks>
    public required bool SchnelleVerbindungen { get; set; }
    
    /// <summary>
    /// Seat reservation only
    /// </summary>
    public required bool SitzplatzOnly { get; set; }
    
    /// <summary>
    /// Bike carriage
    /// </summary>
    public required bool BikeCarriage { get; set; }

    /// <summary>
    /// Only show connections that can be used with the "Deutschlandticket"
    /// </summary>
    public bool NurDeutschlandTicketVerbindungen { get; set; } = false;
    
    /// <summary>
    /// Stopovers
    /// </summary>
    public required List<Zwischenhalt> Zwischenhalte { get; set; }
    
    /// <summary>
    /// Pagination reference for earlier and later connections
    /// </summary>
    public required string? PagingReference { get; set; }
}
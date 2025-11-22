using DBetter.Infrastructure.BahnDe.Connections.DTOs;

namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

public class TeilstreckeAnfrage
{
    /// <summary>
    /// Comfort Class of travel
    /// </summary>
    /// <see cref="Klasse"/>
    public required string Klasse { get; set; }
    
    /// <summary>
    /// Sets the <see cref="AnfrageZeitpunkt"/> to departure or arrival
    /// </summary>
    /// <remarks>Either "ANKUNFT" or "ABFAHRT"</remarks>
    public required string AnkunftSuche { get; set; }
    
    /// <summary>
    /// Allowed vehicles for the journey
    /// </summary>
    /// <see cref="Produktgattungen"/>
    public required List<string> Produktgattungen { get; set; }
    
    /// <summary>
    /// Passengers
    /// </summary>
    public required List<Reisender> Reisende { get; set; }
    
    /// <summary>
    /// Maximum number of transfers on the connection
    /// </summary>
    public required int MaxUmstiege { get; set; }
    
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
    /// Fixed section of the connection that should not change
    /// </summary>
    public required FixedTeilstrecke FixedTeilstrecke { get; set; }
    
    /// <summary>
    /// Id of the original <see cref="Verbindung"/>
    /// </summary>
    public required string OriginalCtxRecon { get; set; }
}
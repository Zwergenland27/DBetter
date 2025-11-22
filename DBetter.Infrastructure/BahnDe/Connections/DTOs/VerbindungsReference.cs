namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Pagination references
/// </summary>
public class VerbindungsReference
{
    /// <summary>
    /// Page reference for earlier connections
    /// </summary>
    /// <remarks>
    /// Not set, when no connections are found
    /// </remarks>
    /// <example>|OB|MTµ14µ133493µ133493µ133517µ133517µ0µ0µ165µ133490µ1µ0µ1275µ0µ0µ-2147483648µ1µ2|PDHµ0942600e008f7e19cfe28818fc3cd943|RDµ15032025|RTµ165300|USµ0|RSµINIT</example>
    public string? Earlier  { get; set; }
    
    /// <summary>
    /// Page reference for later connections
    /// </summary>
    /// <remarks>
    /// Not set when no connections are found
    /// </remarks>
    /// <example>3|OF|MTµ14µ133613µ133613µ133637µ133637µ0µ0µ165µ133596µ5µ0µ1275µ0µ0µ-2147483648µ1µ2|PDHµ0942600e008f7e19cfe28818fc3cd943|RDµ15032025|RTµ165300|USµ0|RSµINIT</example>
    public string? Later { get; set; }
}
namespace DBetter.Infrastructure.BahnDe.Departures;

public class Verkehrsmittel
{
    /// <summary>
    /// Vehicle type
    /// </summary>
    /// <see cref="ProduktGattung"/>
    public required string  ProduktGattung { get; set; }
    
    /// <summary>
    /// Line number of the train
    /// </summary>
    /// <remarks>
    /// For long distance train this is the actual line number and not the train number
    /// </remarks>
    public string? LinienNummer { get; set; }
    
    /// <summary>
    /// Name
    /// </summary>
    /// <example>TL 52973</example>
    public required string Name { get; set; }
    
    /// <summary>
    /// Short name of the train
    /// </summary>
    /// <example>TL</example>
    public required string KurzText { get; set; }
    
    /// <summary>
    /// Medium name of the train
    /// </summary>
    /// <example>TL RB60</example>
    public required string MittelText { get; set; }
    
    /// <summary>
    /// Full name of the train
    /// </summary>
    /// <example>TL RB60 (52973)</example>
    public required string LangText { get; set; }
}
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Information about the transportation method of the section
/// </summary>
public class Verkehrsmittel
{
    /// <summary>
    /// Vehicle type
    /// </summary>
    /// <remarks>
    /// Null if <see cref="Typ"/> is <see cref="VerkehrsmittelTyp.WALK"/>
    /// </remarks>
    /// <see cref="ProduktGattung"/>
    public string?  ProduktGattung { get; set; }
    
    /// <summary>
    /// Name
    /// </summary>
    /// <remarks>
    /// Set to "Walk" for walking section 
    /// </remarks>
    /// <example>TL 52973</example>
    public required string Name { get; set; }
    
    /// <summary>
    /// Train number
    /// </summary>
    /// <remarks>
    /// Null if <see cref="Typ"/> is <see cref="VerkehrsmittelTyp.WALK"/>
    /// </remarks>
    /// <example>52973</example>
    public string? Nummer { get; set; }
    
    /// <summary>
    /// Direction of the train
    /// </summary>
    /// <example>Bautzen</example>
    public string? Richtung { get; set; }
    
    /// <summary>
    /// Type of the transportation method
    /// </summary>
    public required VerkehrsmittelTyp Typ { get; set; }
    
    /// <summary>
    /// Additional information about the train
    /// </summary>
    /// <remarks>
    /// Empty if <see cref="Typ"/> is <see cref="VerkehrsmittelTyp.WALK"/>
    /// </remarks>
    public required List<Zugattribut> Zugattribute { get; set; }
    
    /// <summary>
    /// Short name of the train
    /// </summary>
    /// <remarks>
    /// Null if <see cref="Typ"/> is <see cref="VerkehrsmittelTyp.WALK"/>
    /// </remarks>
    /// <example>TL</example>
    public string? KurzText { get; set; }
    
    /// <summary>
    /// Medium name of the train
    /// </summary>
    /// <remarks>
    /// Null if <see cref="Typ"/> is <see cref="VerkehrsmittelTyp.WALK"/>
    /// </remarks>
    /// <example>TL RB60</example>
    public string? MittelText { get; set; }
    
    /// <summary>
    /// Full name of the train
    /// </summary>
    /// <remarks>
    /// Null if <see cref="Typ"/> is <see cref="VerkehrsmittelTyp.WALK"/>
    /// </remarks>
    /// <example>TL RB60 (52973)</example>
    public string? LangText { get; set; }
}
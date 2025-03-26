namespace DBetter.Infrastructure.BahnDe.Shared;

/// <summary>
/// Prioritized message
/// </summary>
public class PriorisierteMeldung
{
    /// <summary>
    /// Priority
    /// </summary>
    public Prioritaet Prioritaet { get; set; }
    
    /// <summary>
    /// Text of the message
    /// </summary>
    /// <example>TL 52973 departs differently from Dresden Hbf from Platform 13</example>
    public string Text { get; set; }
}
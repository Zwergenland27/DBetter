using System.Runtime.Serialization;

namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.Parameters;

/// <summary>
/// Type of discount
/// </summary>
public enum ArtErmaessigung
{
    /// <summary>
    /// No discount
    /// </summary>
    KEINE_ERMAESSIGUNG,
    BAHNCARD25,
    BAHNCARD50,
    BAHNCARD100
}
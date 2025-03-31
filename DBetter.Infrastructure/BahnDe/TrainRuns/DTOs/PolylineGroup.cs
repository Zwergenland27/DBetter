namespace DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;

/// <summary>
/// Group of poly lines
/// </summary>
public class PolylineGroup
{
    /// <summary>
    /// List of poly line descriptions
    /// </summary>
    public required List<PolyLineDescription> PolyLineDescriptions { get; set; }
}
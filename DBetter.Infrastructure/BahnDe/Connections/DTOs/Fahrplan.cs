using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;

namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Response to <see cref="ReiseAnfrage"/>
/// </summary>
public class Fahrplan
{
    /// <summary>
    /// Suggested connections
    /// </summary>
    public required List<Verbindung> Verbindungen { get; set; }
    
    /// <summary>
    /// Pagination references
    /// </summary>
    public required VerbindungsReference VerbindungReference { get; set; }

    public List<JourneyId> GetJourneyIds()
    {
        return Verbindungen
            .SelectMany(v => v.GetJourneyIds())
            .ToList();
    }

    public List<EvaNumber> GetEvaNumbers(List<JourneyId> journeyIds)
    {
        return Verbindungen
            .SelectMany(v => v.GetEvaNumbers())
            .Distinct()
            .Union(
                journeyIds.Select(id => id.GetDestinationEvaNumber())
                    .Distinct())
            .ToList();
    }
}
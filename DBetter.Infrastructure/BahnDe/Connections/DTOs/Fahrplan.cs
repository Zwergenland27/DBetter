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
            .SelectMany(v => v.VerbindungsAbschnitte)
            .Where(va => va.Verkehrsmittel.Typ is not VerkehrsmittelTyp.WALK)
            .Select(va => new JourneyId(va.JourneyId!))
            .Distinct()
            .ToList();
    }

    public List<EvaNumber> GetEvaNumbers(List<JourneyId> journeyIds)
    {
        return Verbindungen
            .SelectMany(v => v.VerbindungsAbschnitte)
            .SelectMany(va => va.Halte)
            .Select(h => EvaNumber.Create(h.ExtId).Value)
            .Distinct()
            .Union(
                journeyIds.Select(id => id.GetDestinationEvaNumber())
                    .Distinct())
            .ToList();
    }
}
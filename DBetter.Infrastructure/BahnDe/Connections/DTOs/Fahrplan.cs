using DBetter.Domain.Routes.ValueObjects;
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

    public List<BahnJourneyId> GetBahnJourneyIds()
    {
        return Verbindungen
            .SelectMany(v => v.GetJourneyIds())
            .ToList();
    }

    public List<EvaNumber> GetEvaNumbers()
    {
        return Verbindungen
            .SelectMany(v => v.GetEvaNumbers())
            .Distinct()
            .Union(
                GetBahnJourneyIds()
                    .SelectMany(id =>
                    {
                        var parser = new JourneyIdParser(id.Value);
                        return new[]
                        {
                            parser.OriginEvaNumber,
                            parser.DestinationEvaNumber
                        };
                    })
                    .Distinct())
            .ToList();
    }
}
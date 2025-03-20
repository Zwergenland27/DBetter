using DBetter.Application.Connections;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.TrainRun.ValueObjects;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions.Entities;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionsQueryRepository(
    DBetterContext context,
    ConnectionSuggestionService connectionSuggestionService) : IConnectionsQueryRepository
{
    public async Task<List<Connection>> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page)
    {
        var response = await connectionSuggestionService.GetSuggestionsAsync(request, page);

        var journeyIds = response.Verbindungen
            .SelectMany(v => v.VerbindungsAbschnitte)
            .Where(va => va.Verkehrsmittel.Typ is not VerkehrsmittelTyp.WALK)
            .Select(va => new BahnJourneyId(va.JourneyId!))
            .Distinct();

        var existingTrainRuns = await context.TrainRuns
            .Where(tr => journeyIds.Contains(tr.BahnId))
            .ToDictionaryAsync(tr => tr.BahnId.Value, tr => tr);

        List<TrainRunEntity> trainRunsToCreate = [];
        
        var connections = response.Verbindungen.Select(v =>
        {
            var connection = v.ToDomain(request.Id, existingTrainRuns, out var newTrainRuns);
            
            trainRunsToCreate.AddRange(newTrainRuns);
            
            return connection;
        }).ToList();
        
        context.TrainRuns.AddRange(trainRunsToCreate);
        context.Connections.AddRange(connections.Select(ConnectionEntity.FromDomain));
        
        return connections;
    }
}

//VerbindungsReference -> Pagination
//ctxRecon + FixedSection + Anfrage (ohne Start und Zielbahnhof) -> Umstiegszeit verlängern
//=> Umstiegszeit verlängern generiert neue Connection mit eigener Id
//JourneyId -> Gesamtfahrt anzeigen
//ctxRecon + ReisendenAnfrage -> Verbindungsabfrage mit Preisdetails (recon)
//ctxRecon + Neu generierte ReisendenAbfrage -> Verbindung ohne Preisdetails (recon)

//ctxRecon + Abfahrtzeit + Startbahnhof + Zielbahnhof (Name, wie Stationen) -> Bahn Request Id
//=> Einfache Weiterleitung auf Bahn.de, aber Reisendeninfos gehen verloren. Damit sinnlos für Buchung
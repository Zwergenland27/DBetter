using DBetter.Application.Connections;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;
using DBetter.Infrastructure.Postgres;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionsQueryRepository(
    DBetterContext context,
    ConnectionSuggestionService connectionSuggestionService) : IConnectionsQueryRepository
{
    public async Task<List<Connection>> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page)
    {
        var response = await connectionSuggestionService.GetSuggestionsAsync(request, page);
        
        var connections =response.Verbindungen.Select(v => v.ToDomain(request.Id)).ToList();
        
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
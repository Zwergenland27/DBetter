using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.Entities;

public class ConnectionEntity
{
    public ConnectionId Id { get; private init; }
    
    public ContextId ContextId { get; private init; }
    
    private ConnectionEntity(){}

    private ConnectionEntity(
        ConnectionId id,
        ContextId contextId)
    {
        Id = id;
        ContextId = contextId;
    }

    public static ConnectionEntity FromDomain(Connection connection)
    {
        return new ConnectionEntity(
            connection.Id,
            connection.ContextId);
    }
}
/* Anfrage
 *   V
 * Bahn.de DTO Result
 *   V
 * Prüfen, ob BahnJourneyIds bereits für alle Sections existieren
 *   V
 * Bereits vorhandene BahnJourneyIds an die Convertermethoden übergeben
 *   V
 * DTO in Domainobjekte konvertieren
 *   V Konvertierung parallel in Entities
 * Entities speichern
 *   V
 * Ergebnisse zurückgeben
 */
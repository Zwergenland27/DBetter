using System.Text.Json;
using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;

namespace DBetter.Infrastructure.OutboxPattern;

public record OutboxMessage
{
    public Guid Id { get; private init; }
    
    public string Type { get; private init; }
    
    public string Payload { get; private init; }
    public DateTime OccuredAt { get; private init; }
    
    public DateTime? ProcessedAt { get; private set; }

    private OutboxMessage(Guid id, string type, string payload, DateTime occuredAt)
    {
        Id = id;
        Type = type;
        Payload = payload;
        OccuredAt = occuredAt;
    }

    private OutboxMessage(){}

    public static OutboxMessage FromEvent(IDomainEvent @event)
    {
        var type = @event.GetType();
        return new OutboxMessage(
            Guid.NewGuid(),
            $"{type.FullName}, {type.Assembly.GetName().Name}",
            JsonSerializer.Serialize(@event, @event.GetType()),
            DateTime.UtcNow);
    }

    public IDomainEvent ExtractEvent()
    {
        var type = System.Type.GetType(Type);
        return (IDomainEvent) JsonSerializer.Deserialize(Payload, type!)!;
    }

    public void Processed()
    {
        this.ProcessedAt = DateTime.UtcNow;
    }
}
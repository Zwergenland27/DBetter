using CleanMessageBus.Abstractions;
using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.Events;

public record ConnectionContextFlushedEvent(ConnectionId ConnectionId) : IDomainEvent;
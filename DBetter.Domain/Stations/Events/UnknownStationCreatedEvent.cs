using CleanMessageBus.Abstractions;
using DBetter.Domain.Abstractions;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations.Events;

public record UnknownStationCreatedEvent(StationId StationId) : IDomainEvent;
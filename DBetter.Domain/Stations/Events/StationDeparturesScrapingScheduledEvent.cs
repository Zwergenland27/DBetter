using CleanMessageBus.Abstractions;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations.Events;

public record StationDeparturesScrapingScheduledEvent(StationId StationId, DateOnly ForDay): IDomainEvent;
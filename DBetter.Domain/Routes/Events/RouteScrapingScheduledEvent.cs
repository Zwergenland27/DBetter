using CleanMessageBus.Abstractions;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Domain.Routes.Events;

public record RouteScrapingScheduledEvent(RouteId Id) : IDomainEvent;
using CleanMessageBus.Abstractions;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns.Events;

public record TrainRunScrapingScheduledEvent(TrainRunId Id) : IDomainEvent;
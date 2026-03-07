using CleanMessageBus.Abstractions;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Routes.Events;

public record DelayCheckScheduledEvent(TrainRunId OfTrainRun, DateTime ScheduledAt) : IDomainEvent;
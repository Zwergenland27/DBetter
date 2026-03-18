using CleanMessageBus.Abstractions;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCompositions.Events;

public record TrainCompositionUpdateScheduled(TrainRunId TrainRun, DateTime ScheduledAt) : IDomainEvent;
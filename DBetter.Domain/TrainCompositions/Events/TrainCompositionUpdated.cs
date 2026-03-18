using CleanMessageBus.Abstractions;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCompositions.Events;

public record TrainCompositionUpdated(TrainRunId TrainRun): IDomainEvent;
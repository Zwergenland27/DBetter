using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.TrainCompositions.Events;
using DBetter.Domain.TrainCompositions.TrainParts;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCompositions;

public class TrainComposition : AggregateRoot<TrainCompositionId>
{
    private static readonly int[] UpdateIntervalHours = [8, 4, 2, 0];

    private List<PlannedTrainPart> _plannedParts;
    
    private List<ResolvedTrainPart> _resolvedParts;
    
    public TrainRunId TrainRunId { get; private init; }
    
    public IReadOnlyList<ResolvedTrainPart> ResolvedParts => _resolvedParts.AsReadOnly();

    public IReadOnlyList<PlannedTrainPart> PlannedParts => _plannedParts.AsReadOnly();
    
    internal TrainComposition(
        TrainCompositionId id,
        TrainRunId trainRunId,
        List<PlannedTrainPart> plannedParts,
        List<ResolvedTrainPart> resolvedParts) : base(id)
    {
        TrainRunId = trainRunId;
        _plannedParts = plannedParts;
        _resolvedParts = resolvedParts;
    }

    public static TrainComposition Create(TrainRunId trainRunId)
    {
        return new TrainComposition(TrainCompositionId.CreateNew(), trainRunId, [], []);
        //TODO: Raise immediate scheduling event
    }

    public CanFail UpdatePlannedParts(List<PlannedTrainPart> plannedTrainParts)
    {
        if (_resolvedParts.Any()) return TrainCompositionErrors.MorePreciseInformationAvailable;
        _plannedParts.Clear();
        foreach (var plannedTrainPart in plannedTrainParts)
        {
            _plannedParts.Add(plannedTrainPart);
        }
        
        //TODO: Raise scheduling event to gather realtime data
        
        return CanFail.Success;
    }

    public void NoInformationAvailable()
    {
        //TODO: Raise scheduling event to gather realtime data
    }

    public CanFail UpdateResolvedParts(List<ResolvedTrainPart> resolvedParts)
    {
        _resolvedParts.Clear();
        foreach (var resolvedPart in resolvedParts)
        {
            _resolvedParts.Add(resolvedPart);
        }
        return CanFail.Success;
    }
}
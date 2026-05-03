using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using DBetter.Application.Abstractions.RealtimeNotification;
using DBetter.Contracts.TrainCompositions.Get;
using DBetter.Domain.TrainCompositions.Events;
using DBetter.Domain.TrainCompositions.ValueObjects;

namespace DBetter.Application.TrainCompositions.Events;

public class TrainCompositionUpdatedRealtimeNotifier(
    ITrainCompositionQueryRepository trainCompositionQueryRepository,
    IRealtimeNotifier realtimeNotifier) : DomainEventHandlerBase<TrainCompositionUpdated>
{
    public override async Task<CanFail> Handle(TrainCompositionUpdated @event, CancellationToken cancellationToken)
    {
        var newData = await trainCompositionQueryRepository.GetAsync(@event.TrainRun);
        // if(newData is null || newData.Source is TrainFormationSource.None) return CanFail.Success;
        
        await realtimeNotifier.Notify(@event.TrainRun.Value.ToString(), "Update", new GetTrainCompositionResultDto
        {
            LastUpdatedAt = newData.LastUpdatedAt,
            TrainRunId = newData.TrainRunId,
            Source = newData.Source.ToString(),
            Vehicles = newData.Vehicles
        });
        
        return CanFail.Success;
    }
}
using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.Events;

namespace DBetter.Application.Stations.Events;

[Throttled(RequestInterval = 1000)]
public class StationScrapeEventHandler(
    IUnitOfWork unitOfWork,
    IStationRepository stationRepository,
    IStationExternalProvider externalStationProvider) : DomainEventHandlerBase<UnknownStationCreatedEvent>
{
    public override async Task<CanFail> Handle(UnknownStationCreatedEvent @event, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var station = await stationRepository.GetAsync(@event.StationId);
        var stationInfos = await externalStationProvider.GetStationInfoAsync(station.EvaNumber);
        
        station.UpdateInformation(stationInfos);
        await unitOfWork.CommitAsync(cancellationToken);
        return CanFail.Success;
    }
}
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
    IExternalStationProvider externalStationProvider) : DomainEventHandlerBase<UnknownStationCreatedEvent>
{
    public override async Task<CanFail> Handle(UnknownStationCreatedEvent @event, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var station = await stationRepository.GetAsync(@event.StationId);
        if(station is null) return CanFail.Success;
        var stationInfos = await externalStationProvider.GetStationInfoAsync(station.EvaNumber);

        if (stationInfos.InfoId is not null)
        {
            station.Update(stationInfos.InfoId);
        }

        if (stationInfos.Position is not null)
        {
            station.Update(stationInfos.Position);
        }

        if (stationInfos.Ril100 is not null)
        { 
            station.Update(stationInfos.Ril100);   
        }

        if (stationInfos.AvailableMeansOfTransport is not null)
        {
            station.Update(stationInfos.AvailableMeansOfTransport);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
        return CanFail.Success;
    }
}
using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.Events;

namespace DBetter.Application.Stations.Events;

public class StationScrapeEventHandler(
    IUnitOfWork unitOfWork,
    IStationRepository stationRepository,
    IStationInfoProvider stationInfoProvider) : IntegrationEventHandlerBase<UnknownStationCreatedEvent>
{
    public override async Task<CanFail> Handle(UnknownStationCreatedEvent @event, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var station = await stationRepository.GetAsync(@event.StationId);
        var stationInfos = await stationInfoProvider.GetStationInfoAsync(station.EvaNumber);

        if (stationInfos is null)
        {
            throw new NotImplementedException("TODO: Error handling");
        }
        
        station.UpdateInformation(stationInfos);
        await unitOfWork.CommitAsync(cancellationToken);
        return CanFail.Success;
    }
}
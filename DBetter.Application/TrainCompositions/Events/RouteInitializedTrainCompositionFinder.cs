using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Commands;
using CleanMediator.Queries;
using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;
using DBetter.Application.TrainCompositions.Get;
using DBetter.Domain.Routes;
using DBetter.Domain.Routes.Events;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns;

namespace DBetter.Application.TrainCompositions.Events;

[Throttled(RequestInterval = 2000)]
[ConsumedBy(Name = "TrainCompositionFinder")]
public class RouteInitializedTrainCompositionFinder(
    IMediator mediator,
    IRouteRepository routeRepository,
    ITrainRunRepository trainRunRepository,
    ITrainCirculationRepository trainCirculationRepository) : DomainEventHandlerBase<RouteInitializedEvent>
{
    public override async Task<CanFail> Handle(RouteInitializedEvent @event, CancellationToken cancellationToken)
    {
        var route = await routeRepository.GetAsync(@event.Id);
        if (route is null) return CanFail.Success;

        var trainRun = await trainRunRepository.GetAsync(route.TrainRunId);
        if (trainRun is null) throw new InvalidDataException("Train run for route could not be found");

        var trainCirculation = await trainCirculationRepository.GetAsync(trainRun.CirculationId);
        if (trainCirculation is null)
            throw new InvalidDataException("Train circulation for train run could not be found");
        
        if(trainCirculation.ServiceInformation.TransportCategory is not (TransportCategory.HighSpeedTrain or TransportCategory.FastTrain))
            return CanFail.Success;

        var result = await mediator.RunAsync(new GetTrainCompositionQuery(trainRun.Id), cancellationToken);
        if (result.HasFailed)
        {
            if(result.Errors.Count == 1 && result.Errors.First().Code is "TrainComposition.NotFindable" or "TrainComposition.NotSupported" or "TrainComposition.NotFound") return CanFail.Success;
            return result.Errors;
        }
        return CanFail.Success;
    }
}
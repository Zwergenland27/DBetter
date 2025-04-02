using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Domain.Errors;
using DBetter.Domain.Routes;

namespace DBetter.Application.TrainRuns.Queries.Get;

public class GetTrainRunQueryHandler(
    ITrainRunQueryRepository repository) : ICommandHandler<GetTrainRunQuery, Route>
{
    public async Task<CanFail<Route>> Handle(GetTrainRunQuery request, CancellationToken cancellationToken)
    {
        var trainRun = await repository.GetAsync(request.Id);

        if(trainRun is null) return DomainErrors.Route.NotFound(request.Id);
        
        return trainRun;
    }
}
using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Domain.Errors;
using DBetter.Domain.TrainRun;

namespace DBetter.Application.TrainRuns.Queries.Get;

public class GetTrainRunQueryHandler(
    ITrainRunQueryRepository repository) : ICommandHandler<GetTrainRunQuery, TrainRun>
{
    public async Task<CanFail<TrainRun>> Handle(GetTrainRunQuery request, CancellationToken cancellationToken)
    {
        var trainRun = await repository.GetAsync(request.Id);

        if(trainRun is null) return DomainErrors.TrainRun.NotFound(request.Id);
        
        return trainRun;
    }
}
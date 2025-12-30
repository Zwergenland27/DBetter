using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Contracts.Stations.Queries.Find;

namespace DBetter.Application.Stations.Queries.Find;

public class FindStationsQueryHandler(
    IUnitOfWork unitOfWork,
    IStationQueryRepository queryRepository) : QueryHandlerBase<FindStationsQuery, List<StationDto>>
{
    public override async Task<CanFail<List<StationDto>>> Handle(FindStationsQuery request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var results = await queryRepository.FindAsync(request.Query);
        await unitOfWork.CommitAsync(cancellationToken);
        return results;
    }
}
using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Contracts.Stations.Queries.Find;

namespace DBetter.Application.Stations.Queries.Find;

public class FindStationsQueryHandler(IStationQueryRepository queryRepository) : QueryHandlerBase<FindStationsQuery, List<StationDto>>
{
    public override async Task<CanFail<List<StationDto>>> Handle(FindStationsQuery request, CancellationToken cancellationToken)
    {
        return await queryRepository.FindAsync(request.Query);
    }
}
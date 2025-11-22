using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Stations.Queries.Find;
using DBetter.Domain.Stations;

namespace DBetter.Application.Stations.Queries.Find;

public class FindStationsQueryHandler(IStationQueryRepository queryRepository) : IQueryHandler<FindStationsQuery, List<StationDto>>
{
    public async Task<CanFail<List<StationDto>>> Handle(FindStationsQuery request, CancellationToken cancellationToken)
    {
        return await queryRepository.FindAsync(request.Query);
    }
}
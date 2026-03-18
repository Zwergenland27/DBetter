using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using CleanMediator.Queries;
using DBetter.Contracts.Stations.Queries.Find;

namespace DBetter.Application.Stations.Queries.Find;

public class FindStationsRequestBuilder : IRequestBuilder<FindStationParameters, FindStationsQuery>
{
    public ValidatedRequiredProperty<FindStationsQuery> Configure(RequiredPropertyBuilder<FindStationParameters, FindStationsQuery> builder)
    {
        var userId = builder.ClassProperty(r => r.Query)
            .Required()
            .Map(p => p.Query);
        
        return builder.Build(() => new FindStationsQuery(userId));
    }
}


public record FindStationsQuery(string Query) : IQuery<List<StationDto>>;
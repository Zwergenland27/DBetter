using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Contracts.Stations.Queries.Find;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Stations.Queries.Find;

public class FindStationsQueryHandler(
    IUnitOfWork unitOfWork,
    IStationRepository stationRepository,
    IStationExternalProvider externalStationProvider) : QueryHandlerBase<FindStationsQuery, List<StationDto>>
{
    public override async Task<CanFail<List<StationDto>>> Handle(FindStationsQuery request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);

        Station? findByRil100Result = null;
        var ril100CreationResult = Ril100Identifier.Create(request.Query);
        if (!ril100CreationResult.HasFailed)
        {
            findByRil100Result = await stationRepository.FindByRil100Async(ril100CreationResult.Value);
        }
        
        if (findByRil100Result is not null && request.Query.All(char.IsUpper))
        {
            return new List<StationDto>
            {
                findByRil100Result.ToDto()
            };
        }
        
        var fuzzyResults = await externalStationProvider.FindAsync(request.Query);
        var evaNumbersOfResults = fuzzyResults.Select(station => station.EvaNumber);
        var existingStations = await stationRepository.GetManyAsync(evaNumbersOfResults);

        var yetUnknownStations = new List<Station>();

        foreach (var result in fuzzyResults)
        {
            if (existingStations.Any(station => station.EvaNumber == result.EvaNumber)) continue;
            yetUnknownStations.Add(Station.CreateFromSnapshot(result));
        }
        
        stationRepository.AddRange(yetUnknownStations);

        var results = existingStations
            .Select(station => station.ToDto())
            .ToList();
        results.AddRange(yetUnknownStations.Select(station => station.ToDto()));

        results = results
            .OrderBy(station => station.Name)
            .ToList();
        
        if (findByRil100Result is not null)
        {
            results.Insert(0, findByRil100Result.ToDto());
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
        
        return results;
    }
}
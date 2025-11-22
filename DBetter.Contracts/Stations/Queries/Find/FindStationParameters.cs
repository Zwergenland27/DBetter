using CleanDomainValidation.Application;

namespace DBetter.Contracts.Stations.Queries.Find;

public class FindStationParameters : IParameters
{
    /// <summary>
    /// The search term for the station
    /// </summary>
    public string? Query { get; set; }
}
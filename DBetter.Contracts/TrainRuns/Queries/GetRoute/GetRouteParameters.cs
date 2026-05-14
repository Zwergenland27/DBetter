using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.TrainRuns.Queries.GetRoute;

public class GetRouteParameters: IParameters
{
    /// <summary>
    /// Id of the train run
    /// </summary>
    [JsonIgnore]
    public string? Id { get; set; }
}
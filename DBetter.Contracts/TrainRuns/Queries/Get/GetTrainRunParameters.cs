using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.TrainRuns.Queries.Get;

public class GetTrainRunParameters : IParameters
{
    /// <summary>
    /// Id of the train run
    /// </summary>
    [JsonIgnore]
    public string? Id { get; set; }
}
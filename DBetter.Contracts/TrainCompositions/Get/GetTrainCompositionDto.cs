using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.TrainCompositions.Get;

public class GetTrainCompositionDto : IParameters
{
    [JsonIgnore]
    public string? TrainRunId { get; set; }
}
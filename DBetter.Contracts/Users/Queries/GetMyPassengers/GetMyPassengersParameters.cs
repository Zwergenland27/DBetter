using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Users.Queries.GetMyPassengers;

public class GetMyPassengersParameters : IParameters
{
    /// <summary>
    /// Id of the user
    /// </summary>
    [JsonIgnore]
    public string? UserId { get; set; }
}
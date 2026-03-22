using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Infrastructure.TrainCirculations;

public static class TrainCirculationIdentifierExtensions
{
    public static string DatabaseFriendly(this TrainCirculationIdentifier identifier)
    {
        return $"{identifier.Origin.Value}:{identifier.DepartureTime}-{identifier.Destination.Value}:{identifier.Duration.Minutes}";
    }
}
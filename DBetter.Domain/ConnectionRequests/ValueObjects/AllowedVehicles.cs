namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record AllowedVehicles(
    bool HighSpeed,
    bool Intercity,
    bool Regional,
    bool PublicTransport);
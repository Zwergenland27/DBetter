namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record PassengerOptions(
    bool Reservation,
    int Bikes,
    int Dogs,
    bool NeedsAccessibility);
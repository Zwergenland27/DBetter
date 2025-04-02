namespace DBetter.Domain.Route.ValueObjects;

public record TravelTime(
    DateTime Planned,
    DateTime? Real);
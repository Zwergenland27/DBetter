namespace DBetter.Domain.Routes.ValueObjects;

public record TravelTime(
    DateTime Planned,
    DateTime? Real);
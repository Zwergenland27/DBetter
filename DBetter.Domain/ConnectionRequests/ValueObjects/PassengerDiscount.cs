using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record PassengerDiscount(DiscountType Type, Class Class, DateTime? ValidUntil);
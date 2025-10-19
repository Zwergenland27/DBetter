using DBetter.Domain.Shared;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record PassengerDiscount(DiscountType Type, ComfortClass ComfortClass);
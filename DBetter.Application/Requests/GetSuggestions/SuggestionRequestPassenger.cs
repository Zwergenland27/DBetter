using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Requests.GetSuggestions;

public record SuggestionRequestPassenger()
{
    public required int Age { get; init; }
    
    public required int Bikes { get; init; }
    
    public required int Dogs { get; init; }
    
    public required List<PassengerDiscount> Discounts { get; init; }
    
    public bool OwnsDeutschlandTicket => Discounts.Any(d => d.Type is DiscountType.DeutschlandTicket);
}
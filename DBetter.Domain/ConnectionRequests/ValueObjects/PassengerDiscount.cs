using System.Runtime.CompilerServices;
using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;
using DBetter.Domain.Shared;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record PassengerDiscount
{
    private static readonly List<PassengerDiscount> _allowedDiscounts =
    [
        new (DiscountType.BahnCard25, ComfortClass.First),
        new (DiscountType.BahnCard25, ComfortClass.Second),
        new (DiscountType.BahnCard25Business, ComfortClass.First),
        new (DiscountType.BahnCard25Business, ComfortClass.Second),
        new (DiscountType.BahnCard50, ComfortClass.First),
        new (DiscountType.BahnCard50, ComfortClass.Second),
        new (DiscountType.BahnCard50Business, ComfortClass.First),
        new (DiscountType.BahnCard50Business, ComfortClass.Second),
        new (DiscountType.BahnCard100, ComfortClass.First),
        new (DiscountType.BahnCard100, ComfortClass.Second),
        new (DiscountType.CHGeneralAbonnement, ComfortClass.First),
        new (DiscountType.CHGeneralAbonnement, ComfortClass.Second),
        new (DiscountType.HalbtaxAbo, ComfortClass.Unknown),
        new (DiscountType.VorteilsCardAu, ComfortClass.Unknown),
        new (DiscountType.Nl40, ComfortClass.Unknown),
        new (DiscountType.KlimaTicketAu, ComfortClass.Second),
        new (DiscountType.DeutschlandTicket, ComfortClass.Second)
        
    ];
    public DiscountType Type { get; }
    
    public ComfortClass ComfortClass { get; }

    private PassengerDiscount(DiscountType type, ComfortClass comfortClass)
    {
        Type = type;
        ComfortClass = comfortClass;
    }

    public static CanFail<PassengerDiscount> Create(DiscountType type, ComfortClass comfortClass)
    {
        var validCombination = _allowedDiscounts.Any(d => d.Type == type && d.ComfortClass == comfortClass);
        if (!validCombination)
        {
            return DomainErrors.ConnectionRequest.Passenger.Discount.InvalidCombination(type, comfortClass);
        }

        return new PassengerDiscount(type, comfortClass);
    }
}
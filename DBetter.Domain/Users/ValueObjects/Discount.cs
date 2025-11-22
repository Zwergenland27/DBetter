using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;
using DBetter.Domain.Shared;

namespace DBetter.Domain.Users.ValueObjects;

public record Discount
{
    public DiscountType Type { get; private init; }
    
    public ComfortClass ComfortClass { get; private init; }
    
    public DateTime BoughtAtUtc { get; private init; }
    
    public DateTime? ValidUntilUtc { get; private init; }
    
    private Discount(DiscountType type, ComfortClass comfortClass, DateTime boughtAtUtc, DateTime? validUntilUtc)
    {
        Type = type;
        ComfortClass = comfortClass;
        BoughtAtUtc = boughtAtUtc;
        ValidUntilUtc = validUntilUtc;
    }
    
    public static CanFail<Discount> Create(DiscountType type, ComfortClass comfortClass, DateTime boughtAtUtc, DateTime? validUntilUtc)
    {
        return new Discount(type, comfortClass, boughtAtUtc, validUntilUtc);
    }
    
    /// <summary>
    /// Checks if the two coupons can exist without an overlap
    /// </summary>
    /// <param name="other">The other coupon</param>
    public CanFail CanCoExist(Discount other)
    {
        if (other.Type != Type) return CanFail.Success;
        
        if (other.ComfortClass != ComfortClass) return CanFail.Success;
        
        if (ValidUntilUtc is not null && other.BoughtAtUtc >= ValidUntilUtc) return CanFail.Success;

        return DomainErrors.User.Discount.AlreadyExists;
    }
}
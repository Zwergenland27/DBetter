using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Users.ValueObjects;

public record Discount
{
    public DiscountType Type { get; private init; }
    
    public Class Class { get; private init; }
    
    public DateTime BoughtAtUtc { get; private init; }
    
    public DateTime? ValidUntilUtc { get; private init; }
    
    private Discount(DiscountType type, Class @class, DateTime boughtAtUtc, DateTime? validUntilUtc)
    {
        Type = type;
        Class = @class;
        BoughtAtUtc = boughtAtUtc;
        ValidUntilUtc = validUntilUtc;
    }
    
    public static CanFail<Discount> Create(DiscountType type, Class @class, DateTime boughtAtUtc, DateTime? validUntilUtc)
    {
        return new Discount(type, @class, boughtAtUtc, validUntilUtc);
    }
    
    /// <summary>
    /// Checks if the two coupons can exist without an overlap
    /// </summary>
    /// <param name="other">The other coupon</param>
    public CanFail CanCoExist(Discount other)
    {
        if (other.Type != Type) return CanFail.Success;
        
        if (other.Class != Class) return CanFail.Success;
        
        if (ValidUntilUtc is not null && other.BoughtAtUtc >= ValidUntilUtc) return CanFail.Success;

        return DomainErrors.User.Discount.AlreadyExists;
    }
}
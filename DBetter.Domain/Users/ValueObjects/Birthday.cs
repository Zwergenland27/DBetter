using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Users.ValueObjects;

public record Birthday
{
    public DateTime Utc { get; private init; }
    
    private Birthday(DateTime utc)
    {
        Utc = utc;
    }
    
    public static CanFail<Birthday> Create(DateTime utc)
    {
        if (utc >= DateTime.UtcNow) return DomainErrors.User.Birthday.InFuture;
        
        return new Birthday(utc);
    }
}
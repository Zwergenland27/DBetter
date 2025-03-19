using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Shared;

public record Birthday
{
    public DateTime Utc { get; private init; }
    
    private Birthday(DateTime utc)
    {
        Utc = utc;
    }
    
    public static CanFail<Birthday> Create(DateTime utc)
    {
        if (utc >= DateTime.UtcNow) return DomainErrors.Shared.Birthday.InFuture;
        
        return new Birthday(utc);
    }
}
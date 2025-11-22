using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record TransferAmount
{
    public int Value { get; private init; }

    private TransferAmount(int value)
    {
        Value = value;
    }
    
    public static CanFail<TransferAmount> Create(int value)
    {
        if (value < 0)
        {
            return DomainErrors.ConnectionRequest.Route.MaxTransfers.NegativeNotAllowed;
        }
        
        if (value > 10)
        {
            return DomainErrors.ConnectionRequest.Route.MaxTransfers.Max10;
        }

        return new TransferAmount(value);
    }
}
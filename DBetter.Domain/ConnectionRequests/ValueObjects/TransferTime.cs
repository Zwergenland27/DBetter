using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record TransferTime
{
    public int Value { get; private init; }

    private TransferTime(int value)
    {
        Value = value;
    }

    public static CanFail<TransferTime> Create(int value)
    {
        if (value < 0)
        {
            return DomainErrors.ConnectionRequest.Route.MinTransferTime.NegativeNotAllowed;
        }
        if (value > 43)
        {
            return DomainErrors.ConnectionRequest.Route.MinTransferTime.Max43;
        }

        return new TransferTime(value);
    }
}
using DBetter.Domain.Abstractions;
using DBetter.Domain.Errors;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;

namespace DBetter.Domain.PassengerInformationManagement;

public class PassengerInformation : AggregateRoot<PassengerInformationId>
{
    public PassengerInformationType Type { get; private set; }
    
    public PassengerInformationPriority Priority { get; private set; }
    
    public PassengerInformationText Text { get; private set; }
    
    public PassengerInformationCode Code { get; private init; }
    
    private PassengerInformation(
        PassengerInformationId id,
        PassengerInformationType type,
        PassengerInformationPriority priority,
        PassengerInformationText text,
        PassengerInformationCode code) : base(id)
    {
        Type = type;
        Priority = priority;
        Text = text;
        Code = code;
    }
    
    private PassengerInformation() : base(null!){}

    public static PassengerInformation Import(
        PassengerInformationText originalText,
        PassengerInformationCode code,
        PassengerInformationType type,
        PassengerInformationPriority priority)
    {
        return new PassengerInformation(PassengerInformationId.CreateNew(), type, priority, originalText, code);
    }

    public static PassengerInformation FoundNew(
        PassengerInformationText originalText,
        PassengerInformationPriority? priority)
    {
        return new PassengerInformation(PassengerInformationId.CreateNew(), PassengerInformationType.FreeText, priority ?? PassengerInformationPriority.Low, originalText, PassengerInformationCode.Unmapped);
    }
}
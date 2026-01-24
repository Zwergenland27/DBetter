using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Application.Shared;

public class PassengerInformationDto
{
    public required PassengerInformationText OriginalText { get; init; }
    
    public required StopIndex FromStopIndex { get; init; }
    
    public required StopIndex ToStopIndex { get; init; }
    
    public required PassengerInformationPriority? Priority { get; set; }
}
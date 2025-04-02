using DBetter.Domain.Shared;

namespace DBetter.Domain.Route.ValueObjects;

public class RoutePassengerInformation : PassengerInfo
{
    public StopIndex? FromStopIndex { get; set; }
    
    public StopIndex? ToStopIndex { get; set; }

    private RoutePassengerInformation(
        string code,
        string defaultText,
        StopIndex? fromStopIndex,
        StopIndex? toStopIndex) : base(code, defaultText)
    {
        FromStopIndex = fromStopIndex;
        ToStopIndex = toStopIndex;
    }
}
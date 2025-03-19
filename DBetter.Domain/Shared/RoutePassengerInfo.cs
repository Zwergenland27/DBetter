namespace DBetter.Domain.Shared;

public class RoutePassengerInfo : PassengerInfo
{
    public StopIndex? FromStopIndex { get; set; }
    
    public StopIndex? ToStopIndex { get; set; }

    private RoutePassengerInfo(
        string code,
        string defaultText,
        StopIndex? fromStopIndex,
        StopIndex? toStopIndex) : base(code, defaultText)
    {
        FromStopIndex = fromStopIndex;
        ToStopIndex = toStopIndex;
    }
}
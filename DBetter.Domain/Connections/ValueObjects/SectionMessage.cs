namespace DBetter.Domain.Connections.ValueObjects;

public class SectionMessage : Message
{
    public StopIndex? FromStopIndex { get; set; }
    
    public StopIndex? ToStopIndex { get; set; }

    private SectionMessage(
        string code,
        string defaultText,
        StopIndex? fromStopIndex,
        StopIndex? toStopIndex) : base(code, defaultText)
    {
        FromStopIndex = fromStopIndex;
        ToStopIndex = toStopIndex;
    }
}
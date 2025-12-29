namespace DBetter.Domain.Routes.ValueObjects;

public record CateringInformation(
    CateringType Type,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex)
{
    public CateringInformation UpdateStopIndices(StopIndex from, StopIndex to)
    {
        var newFromStopIndex = FromStopIndex;
        var newToStopIndex = ToStopIndex;

        if (from.Value < newFromStopIndex.Value)
        {
            newFromStopIndex = from;
        }

        if (ToStopIndex.Value > newToStopIndex.Value)
        {
            newToStopIndex = to;
        }

        return this with { FromStopIndex = newFromStopIndex, ToStopIndex = newToStopIndex };
    }
}
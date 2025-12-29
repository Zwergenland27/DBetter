namespace DBetter.Domain.Routes.ValueObjects;

public record BikeCarriageInformation(
    BikeCarriageStatus Status,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex)
{
    public BikeCarriageInformation UpdateStopIndices(StopIndex from, StopIndex to)
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
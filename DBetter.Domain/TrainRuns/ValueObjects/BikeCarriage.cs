namespace DBetter.Domain.TrainRuns.ValueObjects;

public record BikeCarriageInformation(
    BikeCarriageStatus Status,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex)
{
    public BikeCarriageInformation Update(BikeCarriageInformation newInformation)
    {
        var newFromStopIndex = FromStopIndex;
        var newToStopIndex = ToStopIndex;

        if (newInformation.FromStopIndex.Value < newFromStopIndex.Value)
        {
            newFromStopIndex = newInformation.FromStopIndex;
        }

        if (newInformation.ToStopIndex.Value > newToStopIndex.Value)
        {
            newToStopIndex = newInformation.ToStopIndex;
        }

        return new BikeCarriageInformation(
            Status: newInformation.Status,
            FromStopIndex: newFromStopIndex,
            ToStopIndex: newToStopIndex);
    }

    public static BikeCarriageInformation Unknown => new (
        BikeCarriageStatus.NoInfo,
        new StopIndex(0),
        new StopIndex(0));
}
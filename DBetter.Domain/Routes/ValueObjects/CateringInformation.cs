namespace DBetter.Domain.Routes.ValueObjects;

public record CateringInformation(
    CateringType Type,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex)
{
    public CateringInformation Update(CateringInformation newInformation)
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

        return new CateringInformation(
            Type: newInformation.Type,
            FromStopIndex: newFromStopIndex,
            ToStopIndex: newToStopIndex);
    }
}
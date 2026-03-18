namespace DBetter.Infrastructure.BahnDe.TrainCompositions.Planned;

public class RootParameters
{
    public DisplayInformation Displayinformation { get; } = new();
    public required Buchungskontext Buchungskontext { get; init; }
    public string CorrelationID { get; } = "";
    public string Lang { get; } = "de";
    public string Theme { get; } = "web";
}
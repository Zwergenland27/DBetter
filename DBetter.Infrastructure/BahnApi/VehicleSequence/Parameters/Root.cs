namespace DBetter.Infrastructure.BahnApi.VehicleSequence.Parameters;

public class Root
{
    public DisplayInformation Displayinformation { get; set; }
    public Buchungskontext Buchungskontext { get; set; }
    public string CorrelationID { get; set; } = "";
    public string Lang { get; set; } = "de";
    public string Theme { get; set; } = "web";
}
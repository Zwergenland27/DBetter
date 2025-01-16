namespace DBetter.Infrastructure.BahnApi.VehicleSequence.Responses;

public class PlannedSequence
{
    public string Type { get; set; }
    
    public string Version { get; set; }
    
    public Zugfahrt Zugfahrt { get; set; }
}
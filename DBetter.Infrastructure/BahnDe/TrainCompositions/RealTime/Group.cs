namespace DBetter.Infrastructure.BahnDe.TrainCompositions.RealTime;

public class Group
{
    public required string Name { get; set; }
    
    public required GroupTransport Transport { get; set; }
    public required List<Vehicle> Vehicles { get; set; }
}
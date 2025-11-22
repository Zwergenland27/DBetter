namespace DBetter.Contracts.Users.Queries.GetMyPassengers;

public class MyPassengersResult
{
    public required PassengerResult Me { get; set; }
    
    public required List<PassengerResult> Family { get; set; }
    
    public required List<PassengerResult> Friends { get; set; }
}
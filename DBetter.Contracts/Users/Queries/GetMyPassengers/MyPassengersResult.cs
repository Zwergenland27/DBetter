namespace DBetter.Contracts.Users.Queries.GetMyPassengers;

public class MyPassengersResult
{
    public PassengerResult Me { get; set; }
    
    public List<PassengerResult> Family { get; set; }
    
    public List<PassengerResult> Friends { get; set; }
}
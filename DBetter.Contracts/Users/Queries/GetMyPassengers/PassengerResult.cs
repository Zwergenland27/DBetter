namespace DBetter.Contracts.Users.Queries.GetMyPassengers;

public class PassengerResult : UserResult
{
    public List<DiscountResult> Discounts { get; set; }
}
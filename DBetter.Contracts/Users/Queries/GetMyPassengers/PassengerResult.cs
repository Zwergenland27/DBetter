namespace DBetter.Contracts.Users.Queries.GetMyPassengers;

public class PassengerResult : IUserResult
{
    public required List<DiscountResult> Discounts { get; set; }
    public required string Id { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required string Email { get; set; }
    public required string Birthday { get; set; }
}
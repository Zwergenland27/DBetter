namespace DBetter.Contracts.Users.Commands.EditPersonalData;

public class EditPersonalDataResult : IUserResult
{
    public required string Id { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required string Email { get; set; }
    
    public required DateTime Birthday { get; set; }
}
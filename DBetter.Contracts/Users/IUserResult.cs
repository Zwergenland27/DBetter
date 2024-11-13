namespace DBetter.Contracts.Users;

public interface IUserResult
{
    /// <summary>
    /// Id of the user
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// First name of the user
    /// </summary>
    /// <example>Max</example>
    public string Firstname { get; set; }
    
    /// <summary>
    /// Last name of the user
    /// </summary>
    /// <example>Mustermann</example>
    public string Lastname { get; set; }
    
    /// <summary>
    /// Email of the user
    /// </summary>
    /// <example>max.mustermann@gmail.com</example>
    public string Email { get; set; }
    
    /// <summary>
    /// Birthday of the user
    /// </summary>
    public DateTime Birthday { get; set; }
}
namespace DBetter.Domain.Connections.ValueObjects;

/// <summary>
/// Customer message
/// </summary>
public class Message
{
    /// <summary>
    /// Custom mapped code
    /// </summary>
    /// <example>Connection.Cancelled</example>
    public string Code { get; private init; }
    
    /// <summary>
    /// Default text from the API
    /// </summary>
    /// <example>
    /// The connection has been cancelled.
    /// </example>
    public string DefaultText { get; private init; }
    
    private Message(){}

    protected Message(
        string code,
        string defaultText)
    {
        Code = code;
        DefaultText = defaultText;
    }
}
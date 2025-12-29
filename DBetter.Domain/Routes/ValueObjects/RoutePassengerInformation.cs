namespace DBetter.Domain.Routes.ValueObjects;

public record RoutePassengerInformation
{
    /// <summary>
    /// Custom mapped code
    /// </summary>
    /// <remarks>
    /// For unknown / yet unmapped codes, the field will be set to FreeText
    /// </remarks>
    public PassengerInformationCode Code { get; private init; }
    
    /// <summary>
    /// Default text from the API when no code could be extracted
    /// </summary>
    /// <example>
    /// The connection has been cancelled.
    /// </example>
    public string? Text { get; private init; }

    private RoutePassengerInformation(
        PassengerInformationCode code)
    {
        Code = code;
    }

    private RoutePassengerInformation(
        string text)
    {
        Code = PassengerInformationCode.FreeText;
        Text = text;
    }
}
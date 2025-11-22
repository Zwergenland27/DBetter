namespace DBetter.Infrastructure.ApiMarketplace;

public class ApiMarketplaceSettings
{
    public const string SectionName = "ApiMarketplaceSettings";
    
    public string ClientId { get; init; } = string.Empty;
    
    public string ApiKey { get; init; } = string.Empty;
}
namespace DBetter.Infrastructure.ApiMarketplace.StaDa.DTOs;

public class Station
{
    public required int Number { get; set; }
    
    public required List<EvaNumber> EvaNumbers { get; set; }
    
    public required List<Ril100Identifier> Ril100Identifiers { get; set; }
}
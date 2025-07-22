namespace DBetter.Infrastructure.ApiMarketplace.StaDa.DTOs;

public class StationQuery
{
    public string? ErrMsg { get; set; }
    
    public int? ErrNo { get; set; }
    
    public List<Station>? Result { get; set; }
    
    public bool HasFailed => ErrNo.HasValue;
}
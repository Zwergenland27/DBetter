namespace DBetter.Contracts.Journeys.DTOs;

public class PriceDto
{
    public float Value { get; set; }
    
    public string Currency { get; set; }
    
    public bool SectionPrice { get; set; }
}
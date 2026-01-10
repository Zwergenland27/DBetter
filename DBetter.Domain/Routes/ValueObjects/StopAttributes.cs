namespace DBetter.Domain.Routes.ValueObjects;

public record StopAttributes
{
    public required bool IsAdditional { get; init; }
    
    public required bool IsCancelled { get; init; }
    
    public required bool IsExitOnly { get; init; }
    
    public required bool IsEntryOnly { get; init; }
    
    public required bool IsRequestStop { get; init; }
}
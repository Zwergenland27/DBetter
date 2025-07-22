namespace DBetter.Domain.Stations.ValueObjects;

public record Ril100
{
    public string Value { get; private init; }

    private Ril100(string value)
    {
        Value = value;
    }
    
    public static Ril100 Create(string value)
    {
        return new Ril100(value);
    }
}
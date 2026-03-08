namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record LineNumber
{
    public string Number { get; private init; }
    
    public string? Prefix { get; private init; }

    private LineNumber(string number, string? prefix)
    {
        Number = number;
        Prefix = prefix;
    }
    public static LineNumber Create(string value)
    {
        var splitted = value.Split(" ");
        if (splitted.Length == 1)
        {
            return new LineNumber(splitted[0], null);
        }
        
        return new LineNumber(splitted[1], splitted[0]);
    }
}
namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record LineNumber
{
    public string Number { get; private init; }
    
    public string? ProductClass { get; private init; }

    internal LineNumber(string number, string? productClass)
    {
        Number = number;
        ProductClass = productClass;
    }

    internal LineNumber Update(LineNumber newLineNumber)
    {
        if (newLineNumber.Number == Number) return this;
        return this with { Number = newLineNumber.Number };
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
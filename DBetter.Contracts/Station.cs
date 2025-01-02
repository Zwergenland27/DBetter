using System.Text.Json.Serialization;

namespace DBetter.Contracts;

public class Station
{
    public String Id { get; set; } = null!;

    public String Name { get; set; } = null!;
}
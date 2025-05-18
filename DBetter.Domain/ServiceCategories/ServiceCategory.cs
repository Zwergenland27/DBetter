using DBetter.Domain.Abstractions;
using DBetter.Domain.ServiceCategories.ValueObjects;

namespace DBetter.Domain.ServiceCategories;

public class ServiceCategory : AggregateRoot<ServiceCategoryId>
{
    public string ShortName { get; private init; }
    
    public string? Name { get; private init; }
    
    public bool CateringExpected { get; private init; }
    
    public bool UsesServiceNumberForPassengers { get; private init; }
    
    private ServiceCategory(
        ServiceCategoryId id,
        string shortName,
        string? name,
        bool cateringExpected,
        bool usesServiceNumberForPassengers) : base(id)
    {
        ShortName = shortName;
        Name = name;
        CateringExpected = cateringExpected;
        UsesServiceNumberForPassengers = usesServiceNumberForPassengers;
    }

    public static ServiceCategory CreateManual(string shortName, string name, bool cateringExpected,
        bool usesServiceNumberForPassengers)
    {
        return new ServiceCategory(
            ServiceCategoryId.CreateNew(), shortName, name, cateringExpected, usesServiceNumberForPassengers);
    }

    public static ServiceCategory CreateDetected(string shortname, bool hasCatering, bool hasLineNumber)
    {
        return new ServiceCategory(
            ServiceCategoryId.CreateNew(), shortname, null, hasCatering, hasLineNumber);
    }
}
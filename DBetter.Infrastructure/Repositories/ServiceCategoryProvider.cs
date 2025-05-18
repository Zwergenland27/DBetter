using DBetter.Domain.ServiceCategories;

namespace DBetter.Infrastructure.Repositories;

public class ServiceCategoryProvider
{
    private readonly List<ServiceCategory> _serviceCategories = [];
    
    public IReadOnlyList<ServiceCategory> ServiceCategories => _serviceCategories.AsReadOnly();
    
    private static readonly HashSet<string> TransportProductsThatUseServiceNumberAsLineNumber =
    [
        "ICE",
        "IC",
        "EC",
        "ECD",
        "EN",
        "ES",
        "RJ",
        "RJX",
        "NJ",
        "TGV",
        "WB",
        "EST",
        "FLX"
    ];

    private static readonly HashSet<string> TransportProductsWithExpectedCatering =
    [
        "ICE",
        "IC",
        "EC",
        "TGV"
    ];
    
    public bool CateringExpected(string serviceCategory)
    {
        return TransportProductsWithExpectedCatering.Contains(serviceCategory);
        //return _serviceCategories.Any(sc => sc.ShortName == serviceCategory && sc.CateringExpected);
    }

    public bool UseServiceNumberAsLineNumber(string serviceCategory)
    {
        return TransportProductsThatUseServiceNumberAsLineNumber.Contains(serviceCategory);
        //return _serviceCategories.Any(sc => sc.ShortName == serviceCategory && sc.UsesServiceNumberForPassengers);
    }
}
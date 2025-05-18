using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.ServiceCategories;
using DBetter.Infrastructure.Repositories;

namespace DBetter.Infrastructure.BahnDe.Shared;

public class RouteInformationFactory
{
    
    private static RouteInformation Create(ServiceCategoryProvider serviceCategoryProvider, string fullName, bool replacementService)
    {
        var information = fullName.Split(' ');

        string serviceCategory = "";
        string? trainLineInfo = null;
        string? numberInfo = null;
        
        //Currently just ferry
        if (information.Length == 1)
        {
            serviceCategory = information[0];
        }
        
        //Trains without dedicated train number
        if (information.Length == 2 && information[0].All(char.IsLetter))
        {
            serviceCategory = information[0];
            trainLineInfo = information[1];
        }
        
        //Trains with train number
        if (information.Length == 2 && information[0].Any(char.IsDigit))
        {
            serviceCategory = new string(information[0].Where(char.IsLetter).ToArray());
            trainLineInfo = new string(information[0].Where(char.IsDigit).ToArray());
            numberInfo = new string(information[1].Where(char.IsDigit).ToArray());
        }
        
        //Trains with train number and without any additional naming
        if (information.Length == 3 && information[1].All(char.IsDigit))
        {
            serviceCategory = information[0];
            trainLineInfo = information[1];
            numberInfo = new string(information[2].Where(char.IsDigit).ToArray());
        }
        
        //Trains with train number that contain an additional name (for example TL for trilex)
        if (information.Length == 3 && information[1].Any(char.IsLetter))
        {
            serviceCategory = new string(information[1].Where(char.IsLetter).ToArray());
            trainLineInfo = new string(information[1].Where(char.IsDigit).ToArray());
            numberInfo = new string(information[2].Where(char.IsDigit).ToArray());
        }
        
        LineNumber? trainLine = null;
        ServiceNumber? trainNumber;
        
        if (serviceCategoryProvider.UseServiceNumberAsLineNumber(serviceCategory))
        {
            trainNumber = GetServiceNumber(trainLineInfo);
        }
        else
        {
            trainLine = GetLineNumber(trainLineInfo);
            trainNumber = GetServiceNumber(numberInfo);
        }
        
        return new RouteInformation(serviceCategory, replacementService, trainLine, trainNumber);
    }

    public static List<RouteInformation> Create(ServiceCategoryProvider serviceCategoryProvider, string leadingVehicleName, string fullName, bool replacementService)
    {
        var fullNames = fullName.Split(" / ")
            .OrderBy(n => !n.Contains(leadingVehicleName))
            .ToList();
        return fullNames.Select(name => Create(serviceCategoryProvider, name, replacementService)).ToList();
    }
    
    public static CateringInformation CreateCateringInformation(ServiceCategoryProvider serviceCategoryProvider, List<Zugattribut>  zugattribute, string serviceCategory, IEnumerable<IRouteStop> stopInfos)
    {
        string? validityText = null;
        CateringType type = CateringType.NoInfo;
        
        if (zugattribute.Any(a => a.Key is "BR"))
        {
            type = CateringType.Restaurant;
            validityText = zugattribute.First(a => a.Key is "BR").Teilstreckenhinweis;
        }

        if (zugattribute.Any(a => a.Key is "QP"))
        {
            type = CateringType.Bistro;
            validityText = zugattribute.First(a => a.Key is "QP").Teilstreckenhinweis;
        }

        if (zugattribute.Any(a => a.Key is "MP"))
        {
            type = CateringType.SeatServed;
            validityText = zugattribute.First(a => a.Key is "MP").Teilstreckenhinweis;
        }

        if (zugattribute.Any(a => a.Key is "SN"))
        {
            type = CateringType.Snack;
            validityText = zugattribute.First(a => a.Key is "SN").Teilstreckenhinweis;
        }

        if (zugattribute.Any(a => a.Key is "KG"))
        {
            type = CateringType.None;
            validityText = zugattribute.First(a => a.Key is "KG").Teilstreckenhinweis;
        }

        if (serviceCategoryProvider.CateringExpected(serviceCategory))
        {
            type =  CateringType.None;
        }

        var partialSectionIndices = GetPartialSectionValidityInfos(validityText, stopInfos);
        
        return new CateringInformation(
            type,
            partialSectionIndices.Item1,
            partialSectionIndices.Item2);
    }

    public static BikeCarriageInformation CreateBikeCarriageInformation(List<Zugattribut>  zugattribute, List<HimMeldung>? himMeldungen, IEnumerable<IRouteStop> stopInfos)
    {
        string? validityText = null;
        BikeCarriageStatus carriageStatus = BikeCarriageStatus.NoInfo;

        if(himMeldungen is null){
            himMeldungen = [];
        }
        
        if (himMeldungen.Any(m => m.Text != null && m.Text.Contains("Die Mitnahme von Fahrrädern ist nicht möglich.")))
        {
            carriageStatus = BikeCarriageStatus.NotPossible;
        }
        
        if (zugattribute.Any(a => a.Key is "FB"))
        {
            carriageStatus = BikeCarriageStatus.Limited;
            validityText = zugattribute.First(a => a.Key is "FB").Teilstreckenhinweis;
        }

        if (zugattribute.Any(a => a.Key is "FR"))
        {
            carriageStatus = BikeCarriageStatus.ReservationRequired;
            validityText = zugattribute.First(a => a.Key is "FR").Teilstreckenhinweis;
        }
        
        var partialSectionIndices = GetPartialSectionValidityInfos(validityText, stopInfos);
        
        return new BikeCarriageInformation(
            carriageStatus,
            partialSectionIndices.Item1,
            partialSectionIndices.Item2);
    }

    public static string? GetOperator(List<Zugattribut> zugattribute)
    {
        return zugattribute.FirstOrDefault(a => a.Key is "BEF")?.Value;
    }

    private static void GetAccessibilityInformation(List<Zugattribut>  zugattribute, IEnumerable<IRouteStop> stopInfos)
    {
        string? validityText = null;
        
        if(zugattribute.Any(a => a.Key is "RZ"))
        {
            //Einstieg mit Rollstuhl stufenfrei
            validityText = zugattribute.First(a => a.Key is "RZ").Teilstreckenhinweis;
        }

        if (zugattribute.Any(a => a.Key is "RH"))
        {
            //Vehicle mounted access aid
            validityText = zugattribute.First(a => a.Key is "RH").Teilstreckenhinweis;
        }
        
        if (zugattribute.Any(a => a.Key is "EA"))
        {
            //Behindertengerechte Ausstattung
            validityText = zugattribute.First(a => a.Key is "EA").Teilstreckenhinweis;
        }
        
        if (zugattribute.Any(a => a.Key is "RG"))
        {
            //Rollstuhlgerechtes Fahrzeug
            validityText = zugattribute.First(a => a.Key is "RG").Teilstreckenhinweis;
        }

        if(zugattribute.Any(a => a.Key is "OC"))
        {
            ///WC accessible for wheelchair
            validityText = zugattribute.First(a => a.Key is "OC").Teilstreckenhinweis;
        }

        if(zugattribute.Any(a => a.Key is "OG"))
        {
            ///WC with limited accessibility for wheelchair
            validityText = zugattribute.First(a => a.Key is "OG").Teilstreckenhinweis;
        }
        
        if (zugattribute.Any(a => a.Key is "RO"))
        {
            //Space for wheelchair
            validityText = zugattribute.First(a => a.Key is "RO").Teilstreckenhinweis;
        }
        
        var partialSectionIndices = GetPartialSectionValidityInfos(validityText, stopInfos);
    }
    
    
    private static Tuple<StopIndex, StopIndex> GetPartialSectionValidityInfos(string? validityText, IEnumerable<IRouteStop> stopInfos)
    {
        var firstStopIndex = new StopIndex(stopInfos.First().RouteIdx);
        var lastStopIndex = new StopIndex(stopInfos.Last().RouteIdx);
        
        if (validityText is null)
        {
            return new (firstStopIndex, lastStopIndex);
        }
        
        var bracesRemoved = validityText.Substring(1, validityText.Length - 2);
        var stationNames =  bracesRemoved.Split(" - ");
        
        firstStopIndex = new StopIndex(stopInfos.First(h => h.Name == stationNames[0]).RouteIdx);
        lastStopIndex = new StopIndex(stopInfos.First(h => h.Name == stationNames[1]).RouteIdx);
        
        return new (firstStopIndex, lastStopIndex);
    }


    private static LineNumber? GetLineNumber(string? lineNumber)
    {
        if (lineNumber is null) return null;
        return new LineNumber(lineNumber);
    }

    public static ServiceNumber? GetServiceNumber(string? serviceNumber)
    {
        if (serviceNumber is null) return null;
        
        if (int.TryParse(serviceNumber, out int result))
        {
            return new ServiceNumber(result);
        }

        return null;
    }
}
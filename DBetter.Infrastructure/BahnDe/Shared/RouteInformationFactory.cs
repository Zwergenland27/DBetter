using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.Shared;

public class RouteInformationFactory
{
    
    private static ServiceInformation Create(TransportCategory category, string fullName)
    {
        var information = fullName.Split(' ');

        string productClass = "";
        string? trainLineInfo = null;
        string? numberInfo = null;
        
        //Currently just ferry and Suburban Trains
        if (information.Length == 1)
        {
            if (information[0].StartsWith('S'))
            {
                productClass = "S";
                trainLineInfo = information[0].Replace("S", "");
            }
            else
            {
                productClass = information[0];
                trainLineInfo = information[0];
            }
        }
        
        //Trains without dedicated train number
        if (information.Length == 2 && information[0].All(char.IsLetter))
        {
            productClass = information[0];
            trainLineInfo = information[1];
        }
        
        //Trains with train number
        if (information.Length == 2 && information[0].Any(char.IsDigit))
        {
            productClass = new string(information[0].Where(char.IsLetter).ToArray());
            trainLineInfo = new string(information[0].Where(char.IsDigit).ToArray());
            numberInfo = new string(information[1].Where(char.IsDigit).ToArray());
        }
        
        //Trains with train number and without any additional naming
        if (information.Length == 3 && information[1].All(char.IsDigit))
        {
            productClass = information[0];
            trainLineInfo = information[1];
            numberInfo = new string(information[2].Where(char.IsDigit).ToArray());
        }
        
        //Trains with train number that contain an additional name (for example TL for trilex)
        if (information.Length == 3 && information[1].Any(char.IsLetter))
        {
            //Remove "IC" from InterCity lines that are also a regional train
            if (information[0] is "RE" && information[1].StartsWith("IC"))
            {
                productClass = information[0];
                trainLineInfo = new string(information[1].Where(char.IsDigit).ToArray());
            }
            else
            {
                productClass = new string(information[1].Where(char.IsLetter).ToArray());
                trainLineInfo = new string(information[1].Where(char.IsDigit).ToArray());    
            }
            
            numberInfo = new string(information[2].Where(char.IsDigit).ToArray());
        }
        
        LineNumber? trainLine = null;
        ServiceNumber? trainNumber;
        
        if (trainLineInfo is null)
        {
            trainNumber = GetServiceNumber(trainLineInfo);
        }
        else
        {
            trainLine = GetLineNumber(trainLineInfo);
            trainNumber = GetServiceNumber(numberInfo);
        }
        
        return new ServiceInformation(category, productClass, trainLine, trainNumber);
    }

    public static List<ServiceInformation> Create(TransportCategory transportCategory, string leadingVehicleName, string fullName)
    {
        var fullNames = fullName.Split(" / ")
            .OrderBy(n => !n.Contains(leadingVehicleName))
            .ToList();
        return fullNames.Select(name => Create(transportCategory, name)).ToList();
    }

    public static string? GetOperator(List<Zugattribut> zugattribute)
    {
        return zugattribute.FirstOrDefault(a => a.Key is "BEF")?.Value;
    }

    // private static void GetAccessibilityInformation(List<Zugattribut>  zugattribute, IEnumerable<IRouteStop> stopInfos)
    // {
    //     string? validityText = null;
    //     
    //     if(zugattribute.Any(a => a.Key is "RZ"))
    //     {
    //         //Einstieg mit Rollstuhl stufenfrei
    //         validityText = zugattribute.First(a => a.Key is "RZ").Teilstreckenhinweis;
    //     }
    //
    //     if (zugattribute.Any(a => a.Key is "RH"))
    //     {
    //         //Vehicle mounted access aid
    //         validityText = zugattribute.First(a => a.Key is "RH").Teilstreckenhinweis;
    //     }
    //     
    //     if (zugattribute.Any(a => a.Key is "EA"))
    //     {
    //         //Behindertengerechte Ausstattung
    //         validityText = zugattribute.First(a => a.Key is "EA").Teilstreckenhinweis;
    //     }
    //     
    //     if (zugattribute.Any(a => a.Key is "RG"))
    //     {
    //         //Rollstuhlgerechtes Fahrzeug
    //         validityText = zugattribute.First(a => a.Key is "RG").Teilstreckenhinweis;
    //     }
    //
    //     if(zugattribute.Any(a => a.Key is "OC"))
    //     {
    //         ///WC accessible for wheelchair
    //         validityText = zugattribute.First(a => a.Key is "OC").Teilstreckenhinweis;
    //     }
    //
    //     if(zugattribute.Any(a => a.Key is "OG"))
    //     {
    //         ///WC with limited accessibility for wheelchair
    //         validityText = zugattribute.First(a => a.Key is "OG").Teilstreckenhinweis;
    //     }
    //     
    //     if (zugattribute.Any(a => a.Key is "RO"))
    //     {
    //         //Space for wheelchair
    //         validityText = zugattribute.First(a => a.Key is "RO").Teilstreckenhinweis;
    //     }
    //     
    //     var partialSectionIndices = GetPartialSectionValidityInfos(validityText, stopInfos);
    // }


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
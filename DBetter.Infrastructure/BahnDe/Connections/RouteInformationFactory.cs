using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections;

public class RouteInformationFactory
{
    private static RouteInformation Create(string fullName)
    {
        var information = fullName.Split(' ');

        string productInfo = "";
        string? trainLineInfo = null;
        string? numberInfo = null;
        
        //Currently just ferry
        if (information.Length == 1)
        {
            productInfo = information[0];
        }
        
        //Trains without dedicated train number
        if (information.Length == 2 && information[0].All(char.IsLetter))
        {
            productInfo = information[0];
            trainLineInfo = information[1];
        }
        
        //Trains with train number
        if (information.Length == 2 && information[0].Any(char.IsDigit))
        {
            productInfo = new String(information[0].Where(char.IsLetter).ToArray());
            trainLineInfo = new String(information[0].Where(char.IsDigit).ToArray());
            numberInfo = new String(information[1].Where(char.IsDigit).ToArray());
        }
        
        //Trains with train number and without any additional naming
        if (information.Length == 3 && information[1].All(char.IsDigit))
        {
            productInfo = information[0];
            trainLineInfo = information[1];
            numberInfo = new String(information[2].Where(char.IsDigit).ToArray());
        }
        
        //Trains with train number that contain an additional name (for example TL for trilex)
        if (information.Length == 3 && information[1].Any(char.IsLetter))
        {
            productInfo = new String(information[1].Where(char.IsLetter).ToArray());
            trainLineInfo = new String(information[1].Where(char.IsDigit).ToArray());
            numberInfo = new String(information[2].Where(char.IsDigit).ToArray());
        }
        
        var product = GetTransportProduct(productInfo);
        LineNumber? trainLine = null;
        ServiceNumber? trainNumber;
        
        if (ServiceNumberIsLineNumber(product))
        {
            trainNumber = GetTrainNumber(trainLineInfo);
        }
        else
        {
            trainLine = GetTrainLine(trainLineInfo);
            trainNumber = GetTrainNumber(numberInfo);
        }
        
        return new RouteInformation(product, trainLine, trainNumber);
    }

    public static List<RouteInformation> Create(string leadingVehicleName, string fullName)
    {
        var fullNames = fullName.Split(" / ")
            .OrderBy(n => !n.Contains(leadingVehicleName))
            .ToList();
        return fullNames.Select(Create).ToList();
    }
    
    private static TransportProduct GetTransportProduct(string info)
    {
        return info switch
        {
            "ICE" => TransportProduct.InterCityExpress,
            "IC" => TransportProduct.InterCity,
            "EC" => TransportProduct.EuroCity,
            "ECD" => TransportProduct.EuroCityDirect,
            "EN" => TransportProduct.EuroNight,
            "ES" => TransportProduct.EuropeanSleeper,
            "RJ" => TransportProduct.RailJet,
            "RJX" => TransportProduct.RailJetExpress,
            "NJ" => TransportProduct.NightJet,
            "TGV" => TransportProduct.TrainAGrandeVitesse,
            "WB" => TransportProduct.WestBahn,
            "EST" => TransportProduct.Eurostar,
            "FLX" => TransportProduct.FlixTrain,
            "IRE" => TransportProduct.InterRegioExpress,
            "MEX" => TransportProduct.MetropolExpress,
            "FEX" => TransportProduct.FlughafenExpress,
            "RE" => TransportProduct.RegionalExpress,
            "RB" => TransportProduct.Regional,
            "S" => TransportProduct.Suburban,
            "Fähre" => TransportProduct.Ferry,
            "U" => TransportProduct.Underground,
            "STR" => TransportProduct.Tram,
            "Bus" => TransportProduct.Bus,
            _ => TransportProduct.Unknown
        };
    }

    public static bool ServiceNumberIsLineNumber(TransportProduct transportProduct)
    {
        return transportProduct switch
        {
            TransportProduct.InterCityExpress => true,
            TransportProduct.InterCity => true,
            TransportProduct.EuroCity => true,
            TransportProduct.EuroCityDirect => true,
            TransportProduct.EuroNight => true,
            TransportProduct.EuropeanSleeper => true,
            TransportProduct.RailJet => true,
            TransportProduct.RailJetExpress => true,
            TransportProduct.NightJet => true,
            TransportProduct.TrainAGrandeVitesse => true,
            TransportProduct.WestBahn => true,
            TransportProduct.Eurostar => true,
            TransportProduct.FlixTrain => true,
            TransportProduct.InterRegioExpress => false,
            TransportProduct.MetropolExpress => false,
            TransportProduct.FlughafenExpress => false,
            TransportProduct.RegionalExpress => false,
            TransportProduct.Regional => false,
            TransportProduct.Suburban => false,
            TransportProduct.Ferry => false,
            TransportProduct.Underground => false,
            TransportProduct.Tram => false,
            TransportProduct.Bus => false,
            TransportProduct.Unknown => false,
            _ => false
        };
    }

    private static bool HasCatering(TransportProduct transportProduct)
    {
        return transportProduct switch
        {
            TransportProduct.InterCityExpress => true,
            TransportProduct.InterCity => true,
            TransportProduct.EuroCity => true,
            TransportProduct.EuroNight => true,
            TransportProduct.EuropeanSleeper => true,
            TransportProduct.RailJet => true,
            TransportProduct.RailJetExpress => true,
            TransportProduct.NightJet => true,
            TransportProduct.TrainAGrandeVitesse => true,
            TransportProduct.WestBahn => true,
            TransportProduct.Eurostar => true,
            TransportProduct.FlixTrain => true,
            TransportProduct.InterRegioExpress => false,
            TransportProduct.MetropolExpress => false,
            TransportProduct.FlughafenExpress => false,
            TransportProduct.RegionalExpress => false,
            TransportProduct.Regional => false,
            TransportProduct.Suburban => false,
            TransportProduct.Ferry => false,
            TransportProduct.Underground => false,
            TransportProduct.Tram => false,
            TransportProduct.Bus => false,
            TransportProduct.Unknown => false,
            _ => false
        };
    }
    
    public static CateringInformation CreateCateringInformation(List<Zugattribut>  zugattribute, TransportProduct product, IEnumerable<ITrainRunStop> stopInfos)
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

        if (HasCatering(product))
        {
            type =  CateringType.None;
        }

        var partialSectionIndices = GetPartialSectionValidityInfos(validityText, stopInfos);
        
        return new CateringInformation(
            type,
            partialSectionIndices.Item1,
            partialSectionIndices.Item2);
    }

    public static BikeCarriageInformation CreateBikeCarriageInformation(List<Zugattribut>  zugattribute, List<HimMeldung>? himMeldungen, IEnumerable<ITrainRunStop> stopInfos)
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

    private static void GetAccessibilityInformation(List<Zugattribut>  zugattribute, IEnumerable<ITrainRunStop> stopInfos)
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
        
        if (zugattribute.Any(a => a.Key is "RO"))
        {
            //Space for wheelchair
            validityText = zugattribute.First(a => a.Key is "RO").Teilstreckenhinweis;
        }
        
        var partialSectionIndices = GetPartialSectionValidityInfos(validityText, stopInfos);
    }
    
    
    private static Tuple<StopIndex, StopIndex> GetPartialSectionValidityInfos(string? validityText, IEnumerable<ITrainRunStop> stopInfos)
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


    private static LineNumber? GetTrainLine(string? trainLine)
    {
        if (trainLine is null) return null;
        return new LineNumber(trainLine);
    }

    public static ServiceNumber? GetTrainNumber(string? trainNumber)
    {
        if (trainNumber is null) return null;
        
        if (int.TryParse(trainNumber, out int result))
        {
            return new ServiceNumber(result);
        }

        return null;
    }
}
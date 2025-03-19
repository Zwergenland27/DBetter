using CleanDomainValidation.Domain;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.TrainRun.ValueObjects;

public record TrainInformation
{
    public TransportProduct Product { get; private init; }
    public TrainLine? Line { get; private init; }
    public TrainNumber? Number { get; private init; }

    private TrainInformation(){}
    
    private TrainInformation(
        TransportProduct Product,
        TrainLine? trainLine,
        TrainNumber? number)
    {
        this.Product = Product;
        Line = trainLine;
        Number = number;
    }

    public static TrainInformation Create(string fullName)
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
        TrainLine? trainLine = null;
        TrainNumber? trainNumber;
        
        if (product.TrainNumberIsLineNumber)
        {
            trainNumber = GetTrainNumber(trainLineInfo);
        }
        else
        {
            trainLine = GetTrainLine(trainLineInfo);
            trainNumber = GetTrainNumber(numberInfo);
        }
        
        return new TrainInformation(product.Product, trainLine, trainNumber);
    }
    
    private static TransportInfo GetTransportProduct(string info)
    {
        return info switch
        {
            "ICE" => new(TransportProduct.InterCityExpress, true),
            "IC" => new(TransportProduct.InterCity, true),
            "EC" => new(TransportProduct.EuroCity, true),
            "EN" => new(TransportProduct.EuroNight, true),
            "ES" => new(TransportProduct.EuropeanSleeper, true),
            "RJ" => new(TransportProduct.RailJet, true),
            "RJX" => new(TransportProduct.RailJetExpress, true),
            "NJ" => new(TransportProduct.NightJet, true),
            "TGV" => new(TransportProduct.TrainAGrandeVitesse, true),
            "WB" => new(TransportProduct.WestBahn, true),
            "EST" => new(TransportProduct.Eurostar, true),
            "FLX" => new(TransportProduct.FlixTrain, true),
            "IRE" => new(TransportProduct.InterRegioExpress, false),
            "MEX" => new(TransportProduct.MetropolExpress, false),
            "FEX" => new(TransportProduct.FlughafenExpress, false),
            "RE" => new(TransportProduct.RegionalExpress, false),
            "RB" => new(TransportProduct.Regional, false),
            "S" => new(TransportProduct.Suburban, false),
            "Fähre" => new(TransportProduct.Ferry, false),
            "U" => new(TransportProduct.Underground, false),
            "STR" => new(TransportProduct.Tram, false),
            "Bus" => new(TransportProduct.Bus, false),
            _ => new(TransportProduct.Unknown, false)
        };
    }

    private static TrainLine? GetTrainLine(string? info)
    {
        if (info is null) return null;
        return new TrainLine(info);
    }

    private static TrainNumber? GetTrainNumber(string? info)
    {
        if (info is null) return null;
        
        if (int.TryParse(info, out int result))
        {
            return new TrainNumber(result);
        }

        return null;
    }
    
    private record TransportInfo(TransportProduct Product, bool TrainNumberIsLineNumber); 
}
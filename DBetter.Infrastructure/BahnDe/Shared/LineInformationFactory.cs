using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Shared;

public class LineInformationFactory(string produktGattung, string fullLineInformation)
{
    private TransportCategory _category;

    private LineNumber? _lineNumber;

    private ServiceNumber? _serviceNumber;

    public ServiceInformation ExtractData()
    {
        _category = Produktgattung.GetTransportCategoryFromAlias(produktGattung);

        var splitLineInformation = fullLineInformation.Split(" ");

        //Suburban trains like S8
        if (_category is TransportCategory.SuburbanTrain && splitLineInformation[0].StartsWith("S"))
        {
            _category = TransportCategory.SuburbanTrain;
            _lineNumber = LineNumber.Create(splitLineInformation[0].Replace("S", "S "));
        }
        //Boats etc.
        else if (_category is TransportCategory.Boat)
        {
            _lineNumber = LineNumber.Create(fullLineInformation);
        }
        //Remove "STR" Prefix for trams
        else if (_category is TransportCategory.Tram)
        {
            _lineNumber = LineNumber.Create(splitLineInformation[1]);
        }
        //Remove "Bus" Prefix for buses
        else if(_category is TransportCategory.Bus)
        {
            _lineNumber = LineNumber.Create(splitLineInformation[1]);
        }
        else if (_category is TransportCategory.Replacement && splitLineInformation[0] is "Bus")
        {
            var lineNumberContent = splitLineInformation[1];
            var productClass = new string(lineNumberContent.TakeWhile(c => !char.IsDigit(c)).ToArray());
            var number = new string(lineNumberContent.SkipWhile(c => !char.IsDigit(c)).ToArray());

            if (number is "")
            {
                _lineNumber = LineNumber.Create(lineNumberContent);
            }
            else
            {
                _lineNumber = LineNumber.Create($"{productClass} {number}");
            }
        }
        //InterCity lines that can also be used with regional ticket
        else if (splitLineInformation.Length > 2 && splitLineInformation[0] is "RE" && splitLineInformation[1].StartsWith("IC"))
        {
            _category = TransportCategory.FastTrain;
            _lineNumber = LineNumber.Create($"RE {new string(splitLineInformation[1].Where(char.IsDigit).ToArray())}");
            if (splitLineInformation.Length == 3 && splitLineInformation[2].Any(char.IsDigit))
            {
                var serviceNumberContent = new string(splitLineInformation[2].Where(char.IsDigit).ToArray());
                _serviceNumber = new ServiceNumber(int.Parse(serviceNumberContent));
            }
        }
        //Trains without dedicated train number
        else if (splitLineInformation.Length == 2 && splitLineInformation[0].All(char.IsLetter))
        {
            _lineNumber = LineNumber.Create($"{splitLineInformation[0]} {splitLineInformation[1]}");
            
            //TODO: line number is sometimes the train number, like in ICEs or TGVs and should be extracted for a correct data model
            //This should not be done using static filters (like only for ices) because sometimes a normal train is also missing its line number
        }
        //Trains with service number and without any additional names
        else if (splitLineInformation.Length == 2 && splitLineInformation[0].Any(char.IsDigit))
        {
            var productClassContent = new string(splitLineInformation[0].Where(char.IsLetter).ToArray());
            var lineNumberContent = new string(splitLineInformation[0].Where(char.IsDigit).ToArray());
            var serviceNumberContent = new string(splitLineInformation[1].Where(char.IsDigit).ToArray());
            
            _lineNumber = LineNumber.Create($"{productClassContent} {lineNumberContent}");
            if (int.TryParse(serviceNumberContent, out var serviceNumber))
            {
                _serviceNumber = new ServiceNumber(serviceNumber);
            }
        }
        //Trains with train number that contain an additional name (for example TL for trilex)
        else if (splitLineInformation.Length == 3 && splitLineInformation[1].All(char.IsDigit))
        {
            var productClassContent = new string(splitLineInformation[1].Where(char.IsLetter).ToArray());
            var lineNumberContent = new string(splitLineInformation[1].Where(char.IsDigit).ToArray());
            var serviceNumberContent = new string(splitLineInformation[2].Where(char.IsDigit).ToArray());
            
            _lineNumber = LineNumber.Create($"{productClassContent} {lineNumberContent}");
            if (int.TryParse(serviceNumberContent, out var serviceNumber))
            {
                _serviceNumber = new ServiceNumber(serviceNumber);
            }
        }
        //Catch all other cases
        else
        {
            _lineNumber = LineNumber.Create(fullLineInformation);
        }
        
        return new ServiceInformation(
            _category,
            _lineNumber,
            _serviceNumber);
    }
}
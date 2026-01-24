using DBetter.Application.Shared;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections;

public class TransportSegmentInformationFactory(VerbindungsAbschnitt verbindungsAbschnitt)
{
    public List<ServiceInformation> ExtractComposition()
    {
        var verkehrsmittel = verbindungsAbschnitt.Verkehrsmittel!;
        
        var composition = new List<ServiceInformation>();
        var leadingPartIdentifier = verkehrsmittel.MittelText!;
        var trainParts = verkehrsmittel.LangText!.Split(" / ");
        var leadingTrainPartInformation = trainParts.First(info => info.Contains(leadingPartIdentifier));
        
        composition.Add(new LineInformationFactory(
            verkehrsmittel.ProduktGattung!,
            leadingTrainPartInformation)
            .ExtractData());

        foreach (var trainPartInformation in trainParts)
        {
            if(trainPartInformation == leadingTrainPartInformation) continue;
            composition.Add( new LineInformationFactory(
                verkehrsmittel.ProduktGattung!,
                trainPartInformation)
                .ExtractData());
        }

        return composition;
    }
    
    public CateringInformation ExtractCateringInformation()
    {
        return new CateringInformationFactory(verbindungsAbschnitt).ExtractInformation();
    }
    
    public BikeCarriageInformation ExtractBikeCarriageInformation()
    {
        return new BikeCarriageInformationFactory(verbindungsAbschnitt).ExtractInformation();
    }

    public List<PassengerInformationDto> ExtractPassengerInformation()
    {
        return new PassengerInformationTextFactory(verbindungsAbschnitt).ExtractInformation();
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
}
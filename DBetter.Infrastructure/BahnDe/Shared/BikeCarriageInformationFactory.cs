using System.Diagnostics.CodeAnalysis;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;

namespace DBetter.Infrastructure.BahnDe.Shared;

public class BikeCarriageInformationFactory(VerbindungsAbschnitt abschnitt)
{
    public BikeCarriageInformation ExtractInformation()
    {
        var type = BikeCarriageStatus.NoInfo;
        Zugattribut? attribute = null;

        if (IsBikeCarriageImpossible())
        {
            type = BikeCarriageStatus.NotPossible;
        }else if (IsBikeReservationRequired(out attribute))
        {
            type =  BikeCarriageStatus.ReservationRequired;
        }else if (IsBikeCarriageLimited(out attribute))
        {
            type = BikeCarriageStatus.Limited;
        }
        
        var partialInformation = new PartialStopIndexFactory(abschnitt.Halte, attribute?.Teilstreckenhinweis);
        return new BikeCarriageInformation(
            type,
            partialInformation.From,
            partialInformation.To);
    }

    private bool IsBikeCarriageImpossible()
    {
        if (abschnitt.HimMeldungen is null) return false;
        return abschnitt.HimMeldungen.Any(info => info.Text is not null && info.Text.Contains("Die Mitnahme von Fahrrädern ist nicht möglich."));
    }
    
    private bool IsBikeReservationRequired([MaybeNullWhen(false) ]out Zugattribut attribute)
    {
        attribute = abschnitt.Verkehrsmittel.Zugattribute.FirstOrDefault(attribute => attribute.Key == "FR");
        return attribute is not null;
    }

    private bool IsBikeCarriageLimited([MaybeNullWhen(false) ]out Zugattribut attribute)
    {
        attribute = abschnitt.Verkehrsmittel.Zugattribute.FirstOrDefault(attribute => attribute.Key == "FB");
        return attribute is not null;
    }
}